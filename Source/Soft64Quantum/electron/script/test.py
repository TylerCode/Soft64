#!/usr/bin/env python

import os
import subprocess
import sys

from lib.util import atom_gyp


SOURCE_ROOT = os.path.abspath(os.path.dirname(os.path.dirname(__file__)))

PROJECT_NAME = atom_gyp()['project_name%']
PRODUCT_NAME = atom_gyp()['product_name%']


def main():
  os.chdir(SOURCE_ROOT)

  config = 'D'
  if len(sys.argv) == 2 and sys.argv[1] == '-R':
    config = 'R'

  if sys.platform == 'darwin':
    atom_shell = os.path.join(SOURCE_ROOT, 'out', config,
                              '{0}.app'.format(PRODUCT_NAME), 'Contents',
                              'MacOS', PRODUCT_NAME)
  elif sys.platform == 'win32':
    atom_shell = os.path.join(SOURCE_ROOT, 'out', config,
                              '{0}.exe'.format(PROJECT_NAME))
  else:
    atom_shell = os.path.join(SOURCE_ROOT, 'out', config, PROJECT_NAME)

  subprocess.check_call([atom_shell, 'spec'] + sys.argv[1:])


if __name__ == '__main__':
  sys.exit(main())
