#!/usr/bin/python
import sys
import os
import re
import shutil

print ("Setting up resources for Soft64UI")

if len(sys.argv) < 2:
	raise ValueError('Invalid number of required command switches')
	
if os.path.isdir(sys.argv[2]) != True:
	raise ValueError('Invalid binary path: ' + sys.argv[2])

minifiedRE = re.compile(".min")
es5RE = re.compile(".es5")
dirBinaryOutput = sys.argv[2]
buildType = sys.argv[1].lower()
print ("Build Type: " + buildType);
isDebug = bool(re.match("debug", buildType))

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
	minifiedFound = bool(minifiedRE.search(basename))
	es5OriginFound = bool(es5RE.search(basename))
	originFile = os.path.dirname(file) + "/" + basename + ".js"
	
	# Skip processing if normal JS file
	if minifiedFound != True and es5OriginFound != True:
		return
		
	# If this is just a .es5.js file and origin exists, remove from list
	if es5OriginFound and originFile in filelist:
		filelist.remove(file)
		return
		
	if minifiedFound and originFile in filelist:
		print("Javascript: Detected minified: " + basename)
	
		if debugMode: # if debug build
			print("Javascript: Removing it from debug build")
			filelist.remove(file) # Then remove it
		else: # Non-debug build
			filelist.remove(originFile) # Remove the original, and keep minified
		
		
fileProcess = {
	".js": processJavascriptFile,
}

#process items in the list
for file in fileCopyList:
	fileName = os.path.splitext(file)[0]
	fileBase = os.path.basename(file)
	fileExt = fileBase[fileBase.rfind("."):]
	
	if fileExt in fileProcess:
		fileProcess[fileExt](fileCopyList, file, fileBase, isDebug)
				
				
# Copy files to target binary

# check if existing target dir exizts
print("\n\n\nCopying Files")
target = dirBinaryOutput + "/HTMLUI"
if os.path.isdir(target):
	shutil.rmtree(target)
os.makedirs(target)

for file in fileCopyList:
	newFilePath = file.replace(htmluiDir, target)
	print(newFilePath)
	#shutil.copyfile(file, newFilePath)
	