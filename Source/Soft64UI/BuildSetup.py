#!/usr/bin/python
import sys
import os

print ("Setting up resources for Soft64UI")

if len(sys.argv) < 2:
	raise ValueError('Invalid number of required command switches')
	
if os.path.isdir(sys.argv[1]) != True:
	raise ValueError('Binary output command switch points to invalid path')
	
dirBinaryOutput = sys.argv[1]
releaseType = sys.argv[0]

# Get working directory
workingDir = os.path.dirname(os.path.realpath(__file__))
htmluiDir = workingDir + "\\HTMLUI"


