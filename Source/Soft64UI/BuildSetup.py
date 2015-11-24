#!/usr/bin/python
# Bryan Perris
# This script setups the html resources for SoftUI builds
# Usage: BuildSetup.py build_mode binary_path

import sys
import os
import re
import shutil
import time

print ("Setting up resources for Soft64UI")

if len(sys.argv) < 2:
	raise ValueError('Invalid number of required command switches')
	
dirBinaryOutput = sys.argv[2].replace("\"", "").replace("\\", "/")

minifiedRE = re.compile(".min")
es5Re = re.compile(".es5")
buildType = sys.argv[1].lower()
print ("Build Type: " + buildType);
isDebug = bool(re.match("debug", buildType))
filesToCopy = []

if isDebug:
	print("Debug build mode is set")

# Get working directory
workingDir = os.path.dirname(os.path.realpath(__file__))
htmluiDir = workingDir + "/HTMLUI"

if os.path.isdir(htmluiDir) != True:
	raise ValueError("HTMLUI directory doesn't exist!")

fileCopyList = []

# Walk through each file of the source HTMLUI directory
for dirName, subdirList, fileList in os.walk(htmluiDir):
	for fname in fileList:
		fileCopyList.append(dirName + "/" + fname)
		
def processJavascriptFile(filelist, file, basename, debugMode):
	if bool(es5Re.search(basename)):
		return

	minifiedFound = bool(minifiedRE.search(basename))
	originFile = file.split('.')[0] + ".js"
	
	if minifiedFound and originFile in filelist:
		print("Javascript: Detected minified: " + basename)
	
		if debugMode:
			print("Javascript: File copy skipped for debug build")
			filesToCopy.append(originFile)
		else:
			filesToCopy.append(file)
	else:
		filesToCopy.append(file)
		

def removeFile(filelist, file, basename, debugMode):
	return
	
def processCssFile(filelist, file, basename, debugMode):
	minifiedFound = bool(minifiedRE.search(basename))
	originFile = file.split('.')[0] + ".css"
	
	if minifiedFound and originFile in filelist:
		print("CSS: Detected minified: " + basename)
	
		if debugMode:
			print("CSS: File copy skipped for debug build")
			filesToCopy.append(originFile)
		else:
			filesToCopy.append(file)
	else:
		filesToCopy.append(file)
		
fileProcess = {
	".js": processJavascriptFile,
	".less": removeFile,
	".css": processCssFile,
}

#process items in the list
for file in fileCopyList:
	fileName = os.path.splitext(file)[0]
	fileBase = os.path.basename(file)
	fileExt = fileBase[fileBase.rfind("."):]
	
	if fileExt in fileProcess:
		fileProcess[fileExt](fileCopyList, file, fileBase, isDebug)
	else:
		filesToCopy.append(file)
				
				
# Copy files to target binary
def copyFile(targetDir, sourceFile, destFilePath, filename):
	split1 = destFilePath.replace("/" + filename, "").split("HTMLUI")[1].replace("\\", "/")
	split2 = split1.split("/")
	
	for dir in split2:
		if dir:
			targetDir = targetDir + "/" + dir
			if not os.path.isdir(targetDir):
				os.makedirs(targetDir)
	
	shutil.copy(sourceFile, os.path.dirname(destFilePath))

# check if existing target dir exizts
print("\n\nCopying Files")
target = dirBinaryOutput + "/HTMLUI"
if os.path.isdir(target):
	shutil.rmtree(target, ignore_errors=True)

time.sleep(3)
if not os.path.isdir(target):
	os.makedirs(target)

for file in filesToCopy:
	newFilePath = file.replace(htmluiDir, target)
	print(newFilePath)
	copyFile(target, file, newFilePath, os.path.basename(file))

	
modejs = open(target + "/js/app/mode.js", "w")
modejs.write("var mode = \"" + buildType + "\"")
modejs.close()

sys.exit(0)
	