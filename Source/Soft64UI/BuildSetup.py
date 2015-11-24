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
		
def processFile(ext, filelist, basename, debugMode):
	minifiedFound = bool(minifiedRE.search(basename))
	originFile = file.split('.')[0] + ext
	
	# If we are processing the orignal source file, add it to the list for now
	if not minifiedFound and originFile in filelist:
		filesToCopy.append(file)
	else:
		# if the file is minified, do some checks
		if minifiedFound and originFile in filelist:
			if debugMode:
				if not originFile in filesToCopy:
					# In debug mode, copy the original file if doesn't exist yet
					filesToCopy.append(originFile)
			else:
				# Use the minified version and remove original copy if its there
				if originFile in fileCopyList:
					filesToCopy.remove(originFile);
				filesToCopy.append(file)
		
def processJavascriptFile(filelist, file, basename, debugMode):
	if bool(es5Re.search(basename)):
		return

	processFile(".js", filelist, basename, debugMode)
		

def removeFile(filelist, file, basename, debugMode):
	return
	
def processCssFile(filelist, file, basename, debugMode):
	processFile(".css", filelist, basename, debugMode)
		
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
	