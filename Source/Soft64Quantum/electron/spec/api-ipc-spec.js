'use strict'

const assert = require('assert')
const path = require('path')

const ipcRenderer = require('electron').ipcRenderer
const remote = require('electron').remote

const ipcMain = remote.require('electron').ipcMain
const BrowserWindow = remote.require('electron').BrowserWindow

const comparePaths = function (path1, path2) {
  if (process.platform === 'win32') {
    path1 = path1.toLowerCase()
    path2 = path2.toLowerCase()
  }
  assert.equal(path1, path2)
}

describe('ipc module', function () {
  var fixtures = path.join(__dirname, 'fixtures')

  describe('remote.require', function () {
    it('should returns same object for the same module', function () {
      var dialog1 = remote.require('electron')
      var dialog2 = remote.require('electron')
      assert.equal(dialog1, dialog2)
    })

    it('should work when object contains id property', function () {
      var a = remote.require(path.join(fixtures, 'module', 'id.js'))
      assert.equal(a.id, 1127)
    })

    it('should search module from the user app', function () {
      comparePaths(path.normalize(remote.process.mainModule.filename), path.resolve(__dirname, 'static', 'main.js'))
      comparePaths(path.normalize(remote.process.mainModule.paths[0]), path.resolve(__dirname, 'static', 'node_modules'))
    })
  })

  describe('remote.createFunctionWithReturnValue', function () {
    it('should be called in browser synchronously', function () {
      var buf = new Buffer('test')
      var call = remote.require(path.join(fixtures, 'module', 'call.js'))
      var result = call.call(remote.createFunctionWithReturnValue(buf))
      assert.equal(result.constructor.name, 'Buffer')
    })
  })

  describe('remote object in renderer', function () {
    it('can change its properties', function () {
      var property = remote.require(path.join(fixtures, 'module', 'property.js'))
      assert.equal(property.property, 1127)
      property.property = 1007
      assert.equal(property.property, 1007)
      var property2 = remote.require(path.join(fixtures, 'module', 'property.js'))
      assert.equal(property2.property, 1007)
      property.property = 1127
    })

    it('can construct an object from its member', function () {
      var call = remote.require(path.join(fixtures, 'module', 'call.js'))
      var obj = new call.constructor()
      assert.equal(obj.test, 'test')
    })

    it('can reassign and delete its member functions', function () {
      var remoteFunctions = remote.require(path.join(fixtures, 'module', 'function.js'))
      assert.equal(remoteFunctions.aFunction(), 1127)

      remoteFunctions.aFunction = function () { return 1234 }
      assert.equal(remoteFunctions.aFunction(), 1234)

      assert.equal(delete remoteFunctions.aFunction, true)
    })

    it('is referenced by its members', function () {
      let stringify = remote.getGlobal('JSON').stringify
      gc();
      stringify({})
    });
  })

  describe('remote value in browser', function () {
    var print = path.join(fixtures, 'module', 'print_name.js')

    it('keeps its constructor name for objects', function () {
      var buf = new Buffer('test')
      var print_name = remote.require(print)
      assert.equal(print_name.print(buf), 'Buffer')
    })

    it('supports instanceof Date', function () {
      var now = new Date()
      var print_name = remote.require(print)
      assert.equal(print_name.print(now), 'Date')
      assert.deepEqual(print_name.echo(now), now)
    })
  })

  describe('remote promise', function () {
    it('can be used as promise in each side', function (done) {
      var promise = remote.require(path.join(fixtures, 'module', 'promise.js'))
      promise.twicePromise(Promise.resolve(1234)).then(function (value) {
        assert.equal(value, 2468)
        done()
      })
    })
  })

  describe('remote webContents', function () {
    it('can return same object with different getters', function () {
      var contents1 = remote.getCurrentWindow().webContents
      var contents2 = remote.getCurrentWebContents()
      assert(contents1 === contents2)
    })
  })

  describe('remote class', function () {
    let cl = remote.require(path.join(fixtures, 'module', 'class.js'))
    let base = cl.base
    let derived = cl.derived

    it('can get methods', function () {
      assert.equal(base.method(), 'method')
    })

    it('can get properties', function () {
      assert.equal(base.readonly, 'readonly')
    })

    it('can change properties', function () {
      assert.equal(base.value, 'old')
      base.value = 'new'
      assert.equal(base.value, 'new')
      base.value = 'old'
    })

    it('has unenumerable methods', function () {
      assert(!base.hasOwnProperty('method'))
      assert(Object.getPrototypeOf(base).hasOwnProperty('method'))
    })

    it('keeps prototype chain in derived class', function () {
      assert.equal(derived.method(), 'method')
      assert.equal(derived.readonly, 'readonly')
      assert(!derived.hasOwnProperty('method'))
      let proto = Object.getPrototypeOf(derived)
      assert(!proto.hasOwnProperty('method'))
      assert(Object.getPrototypeOf(proto).hasOwnProperty('method'))
    })

    it('is referenced by methods in prototype chain', function () {
      let method = derived.method
      derived = null
      gc()
      assert.equal(method(), 'method')
    });
  })

  describe('ipc.sender.send', function () {
    it('should work when sending an object containing id property', function (done) {
      var obj = {
        id: 1,
        name: 'ly'
      }
      ipcRenderer.once('message', function (event, message) {
        assert.deepEqual(message, obj)
        done()
      })
      ipcRenderer.send('message', obj)
    })

    it('can send instance of Date', function (done) {
      const currentDate = new Date()
      ipcRenderer.once('message', function (event, value) {
        assert.equal(value, currentDate.toISOString())
        done()
      })
      ipcRenderer.send('message', currentDate)
    })
  })

  describe('ipc.sendSync', function () {
    it('can be replied by setting event.returnValue', function () {
      var msg = ipcRenderer.sendSync('echo', 'test')
      assert.equal(msg, 'test')
    })

    it('does not crash when reply is not sent and browser is destroyed', function (done) {
      this.timeout(10000)

      var w = new BrowserWindow({
        show: false
      })
      ipcMain.once('send-sync-message', function (event) {
        event.returnValue = null
        w.destroy()
        done()
      })
      w.loadURL('file://' + path.join(fixtures, 'api', 'send-sync-message.html'))
    })
  })

  describe('remote listeners', function () {
    var w = null

    afterEach(function () {
      w.destroy()
    })

    it('can be added and removed correctly', function () {
      w = new BrowserWindow({
        show: false
      })
      var listener = function () {}
      w.on('test', listener)
      assert.equal(w.listenerCount('test'), 1)
      w.removeListener('test', listener)
      assert.equal(w.listenerCount('test'), 0)
    })
  })
})
