# Soft64
Emulator

# Requirements
 * Node.JS 4.x
  * Global packages
    * jshint (optional)
    * grunt-cli
    * node-gyp

 * Mono 4.x  or Microsoft .NET 4.5 (CoreCLR not tested or supported yet)
 * Mono/.NET development tools
 
# Instructions to build and run
 * Clone the git source
 * ```cd``` into the directory, and execute: ```npm update```
 * Build cli only: ```grunt simple```
 * Build everything: ```grunt```
 * To run the cli frontend: ```npm start```
 * To run the gui frontend: ```npm run-script start-ui```
 * To build with debug flags (eg. debug C# code), add ```--Debug``` to end of the ```grunt``` command



# Linux tips
If you are working with the edge node package on Linux, you must at least install Mono 4.2 or higher, else edge faults.
I used this command on Ubuntu to add their GPG keys and sofware source:

```
sudo apt-key adv --keyserver hkp://keyserver.ubuntu.com:80 --recv-keys 3FA7E0328081BFF6A14DA29AA6A19B38D3D831EF
echo "deb http://download.mono-project.com/repo/debian wheezy main" | sudo tee /etc/apt/sources.list.d/mono-xamarin.list
sudo apt-get update
```

Then install monodevelop to get all mono dev packages installed.
