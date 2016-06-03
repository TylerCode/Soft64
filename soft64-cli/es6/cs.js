/*jshint esversion:6 */

import edge from 'edge';

class cs {
  init() {
      var f = edge.func(function () {
      /*
        #r "mscorlib.dll"

        using System;
        using System.Reflection;
        using System.Threading.Tasks;

        public class Startup
        {
          public async Task<Object> Invoke(Object input)
          {
            Assembly coreAssembly = Assembly.LoadFrom("Soft64/Soft64.dll");
           return String.Format("Result of load: {0}", coreAssembly.ToString());
          }
        }
      */
    });

    f('', function (err, res) {
      if (err) throw err;
      console.log(res);
    });
  }
}

module.exports = cs;
