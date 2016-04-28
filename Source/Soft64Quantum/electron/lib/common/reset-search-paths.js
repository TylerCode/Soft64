const path = require('path')
const Module = require('module')

// Clear Node's global search paths.
Module.globalPaths.length = 0

// Clear current and parent(init.coffee)'s search paths.
module.paths = []

module.parent.paths = []

// Prevent Node from adding paths outside this app to search paths.
Module._nodeModulePaths = function (from) {
  var dir, i, part, parts, paths, skipOutsidePaths, splitRe, tip
  from = path.resolve(from)

  // If "from" is outside the app then we do nothing.
  skipOutsidePaths = from.startsWith(process.resourcesPath)

  // Following logoic is copied from module.js.
  splitRe = process.platform === 'win32' ? /[\/\\]/ : /\//
  paths = []
  parts = from.split(splitRe)
  for (tip = i = parts.length - 1; i >= 0; tip = i += -1) {
    part = parts[tip]
    if (part === 'node_modules') {
      continue
    }
    dir = parts.slice(0, tip + 1).join(path.sep)
    if (skipOutsidePaths && !dir.startsWith(process.resourcesPath)) {
      break
    }
    paths.push(path.join(dir, 'node_modules'))
  }
  return paths
}
