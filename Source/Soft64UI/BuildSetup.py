#!/usr/bin/python
import sys
import os
import re

print ("Setting up resources for Soft64UI")

if len(sys.argv) < 2:
	raise ValueError('Invalid number of required command switches')
	
if os.path.isdir(sys.argv[1]) != True:
	raise ValueError('Binary output command switch points to invalid path')
	
dirBinaryOutput = sys.argv[1]
releaseType = sys.argv[0].lower()

isDebug = releaseType is "debug"

# Get working directory
workingDir = os.path.dirname(os.path.realpath(__file__))
htmluiDir = workingDir + "\\HTMLUI"

fileCopyList = []

# Walk through each file of the source HTMLUI directory
for dirName, subdirList, fileList in os.walk(rootDir):
	for fname in fileList:
        fileCopyList.append(fname)

#process items in the list
for file in fileCopyList:
	fileName = os.path.splitext(file)[0]
	fileBase = os.path.basename(file)
	fileExt = fileBase[fileBase.rfind("."):]
	print("Processing File: " + fileName)
	if isDebug and fileBase.match(".min."):
		orginFile = file.replace(".min.", "")
		if orginFile in fileCopyList:
			fileCopyList.remove(orginFile)