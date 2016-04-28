# Mac App Store Submission Guide

Since v0.34.0, Electron allows submitting packaged apps to the Mac App Store
(MAS). This guide provides information on: how to submit your app and the
limitations of the MAS build.

**Note:** Submitting an app to Mac App Store requires enrolling [Apple Developer
Program][developer-program], which costs money.

## How to Submit Your App

The following steps introduce a simple way to submit your app to Mac App Store.
However, these steps do not ensure your app will be approved by Apple; you
still need to read Apple's [Submitting Your App][submitting-your-app] guide on
how to meet the Mac App Store requirements.

### Get Certificate

To submit your app to the Mac App Store, you first must get a certificate from
Apple. You can follow these [existing guides][nwjs-guide] on web.

### Sign Your App

After getting the certificate from Apple, you can package your app by following
[Application Distribution](application-distribution.md), and then proceed to
signing your app. This step is basically the same with other programs, but the
key is to sign every dependency of Electron one by one.

First, you need to prepare two entitlements files.

`child.plist`:

```xml
<?xml version="1.0" encoding="UTF-8"?>
<!DOCTYPE plist PUBLIC "-//Apple//DTD PLIST 1.0//EN" "http://www.apple.com/DTDs/PropertyList-1.0.dtd">
<plist version="1.0">
  <dict>
    <key>com.apple.security.app-sandbox</key>
    <true/>
    <key>com.apple.security.inherit</key>
    <true/>
  </dict>
</plist>
```

`parent.plist`:

```xml
<?xml version="1.0" encoding="UTF-8"?>
<!DOCTYPE plist PUBLIC "-//Apple//DTD PLIST 1.0//EN" "http://www.apple.com/DTDs/PropertyList-1.0.dtd">
<plist version="1.0">
  <dict>
    <key>com.apple.security.app-sandbox</key>
    <true/>
    <key>com.apple.security.temporary-exception.sbpl</key>
    <string>(allow mach-lookup (global-name-regex #"^org.chromium.Chromium.rohitfork.[0-9]+$"))</string>
  </dict>
</plist>
```

And then sign your app with the following script:

```bash
#!/bin/bash

# Name of your app.
APP="YourApp"
# The path of you app to sign.
APP_PATH="/path/to/YouApp.app"
# The path to the location you want to put the signed package.
RESULT_PATH="~/Desktop/$APP.pkg"
# The name of certificates you requested.
APP_KEY="3rd Party Mac Developer Application: Company Name (APPIDENTITY)"
INSTALLER_KEY="3rd Party Mac Developer Installer: Company Name (APPIDENTITY)"

FRAMEWORKS_PATH="$APP_PATH/Contents/Frameworks"

codesign -s "$APP_KEY" -f --entitlements child.plist "$FRAMEWORKS_PATH/Electron Framework.framework/Versions/A/Electron Framework"
codesign -s "$APP_KEY" -f --entitlements child.plist "$FRAMEWORKS_PATH/Electron Framework.framework/Versions/A/Libraries/libffmpeg.dylib"
codesign -s "$APP_KEY" -f --entitlements child.plist "$FRAMEWORKS_PATH/Electron Framework.framework/Versions/A/Libraries/libnode.dylib"
codesign -s "$APP_KEY" -f --entitlements child.plist "$FRAMEWORKS_PATH/Electron Framework.framework"
codesign -s "$APP_KEY" -f --entitlements child.plist "$FRAMEWORKS_PATH/$APP Helper.app/Contents/MacOS/$APP Helper"
codesign -s "$APP_KEY" -f --entitlements child.plist "$FRAMEWORKS_PATH/$APP Helper.app/"
codesign -s "$APP_KEY" -f --entitlements child.plist "$FRAMEWORKS_PATH/$APP Helper EH.app/Contents/MacOS/$APP Helper EH"
codesign -s "$APP_KEY" -f --entitlements child.plist "$FRAMEWORKS_PATH/$APP Helper EH.app/"
codesign -s "$APP_KEY" -f --entitlements child.plist "$FRAMEWORKS_PATH/$APP Helper NP.app/Contents/MacOS/$APP Helper NP"
codesign -s "$APP_KEY" -f --entitlements child.plist "$FRAMEWORKS_PATH/$APP Helper NP.app/"
codesign -s "$APP_KEY" -f --entitlements child.plist "$APP_PATH/Contents/MacOS/$APP"
codesign -s "$APP_KEY" -f --entitlements parent.plist "$APP_PATH"

productbuild --component "$APP_PATH" /Applications --sign "$INSTALLER_KEY" "$RESULT_PATH"
```

If you are new to app sandboxing under OS X, you should also read through
Apple's [Enabling App Sandbox][enable-app-sandbox] to have a basic idea, then
add keys for the permissions needed by your app to the entitlements files.

### Upload Your App

After signing your app, you can use Application Loader to upload it to iTunes
Connect for processing, making sure you have [created a record][create-record]
before uploading.

### Explain the Usages of `temporary-exception`

When sandboxing your app there was a `temporary-exception` entry added to the
entitlements, according to the [App Sandbox Temporary Exception
Entitlements][temporary-exception] documentation, you have to explain why this
entry is needed:

> Note: If you request a temporary-exception entitlement, be sure to follow the
guidance regarding entitlements provided on the iTunes Connect website. In
particular, identify the entitlement and corresponding issue number in the App
Sandbox Entitlement Usage Information section in iTunes Connect and explain why
your app needs the exception.

You may explain that your app is built upon Chromium browser, which uses Mach
port for its multi-process architecture. But there is still probability that
your app failed the review because of this.

### Submit Your App for Review

After these steps, you can [submit your app for review][submit-for-review].

## Limitations of MAS Build

In order to satisfy all requirements for app sandboxing, the following modules
have been disabled in the MAS build:

* `crashReporter`
* `autoUpdater`

and the following behaviors have been changed:

* Video capture may not work for some machines.
* Certain accessibility features may not work.
* Apps will not be aware of DNS changes.

Also, due to the usage of app sandboxing, the resources which can be accessed by
the app are strictly limited; you can read [App Sandboxing][app-sandboxing] for
more information.

## Cryptographic Algorithms Used by Electron

Depending on the country and region you are located, Mac App Store may require
documenting the cryptographic algorithms used in your app, and even ask you to
submit a copy of U.S. Encryption Registration (ERN) approval.

Electron uses following cryptographic algorithms:

* AES - [NIST SP 800-38A](http://csrc.nist.gov/publications/nistpubs/800-38a/sp800-38a.pdf), [NIST SP 800-38D](http://csrc.nist.gov/publications/nistpubs/800-38D/SP-800-38D.pdf), [RFC 3394](http://www.ietf.org/rfc/rfc3394.txt)
* HMAC - [FIPS 198-1](http://csrc.nist.gov/publications/fips/fips198-1/FIPS-198-1_final.pdf)
* ECDSA - ANS X9.62–2005
* ECDH - ANS X9.63–2001
* HKDF - [NIST SP 800-56C](http://csrc.nist.gov/publications/nistpubs/800-56C/SP-800-56C.pdf)
* PBKDF2 - [RFC 2898](https://tools.ietf.org/html/rfc2898)
* RSA - [RFC 3447](http://www.ietf.org/rfc/rfc3447)
* SHA - [FIPS 180-4](http://csrc.nist.gov/publications/fips/fips180-4/fips-180-4.pdf)
* Blowfish - https://www.schneier.com/cryptography/blowfish/
* CAST - [RFC 2144](https://tools.ietf.org/html/rfc2144), [RFC 2612](https://tools.ietf.org/html/rfc2612)
* DES - [FIPS 46-3](http://csrc.nist.gov/publications/fips/fips46-3/fips46-3.pdf)
* DH - [RFC 2631](https://tools.ietf.org/html/rfc2631)
* DSA - [ANSI X9.30](http://webstore.ansi.org/RecordDetail.aspx?sku=ANSI+X9.30-1%3A1997)
* EC - [SEC 1](http://www.secg.org/sec1-v2.pdf)
* IDEA - "On the Design and Security of Block Ciphers" book by X. Lai
* MD2 - [RFC 1319](http://tools.ietf.org/html/rfc1319)
* MD4 - [RFC 6150](https://tools.ietf.org/html/rfc6150)
* MD5 - [RFC 1321](https://tools.ietf.org/html/rfc1321)
* MDC2 - [ISO/IEC 10118-2](https://www.openssl.org/docs/manmaster/crypto/mdc2.html)
* RC2 - [RFC 2268](https://tools.ietf.org/html/rfc2268)
* RC4 - [RFC 4345](https://tools.ietf.org/html/rfc4345)
* RC5 - http://people.csail.mit.edu/rivest/Rivest-rc5rev.pdf
* RIPEMD - [ISO/IEC 10118-3](http://webstore.ansi.org/RecordDetail.aspx?sku=ISO%2FIEC%2010118-3:2004)

On how to get the ERN approval, you can reference the article: [How to legally
submit an app to Apple’s App Store when it uses encryption (or how to obtain an
ERN)][ern-tutorial].

[developer-program]: https://developer.apple.com/support/compare-memberships/
[submitting-your-app]: https://developer.apple.com/library/mac/documentation/IDEs/Conceptual/AppDistributionGuide/SubmittingYourApp/SubmittingYourApp.html
[nwjs-guide]: https://github.com/nwjs/nw.js/wiki/Mac-App-Store-%28MAS%29-Submission-Guideline#first-steps
[enable-app-sandbox]: https://developer.apple.com/library/ios/documentation/Miscellaneous/Reference/EntitlementKeyReference/Chapters/EnablingAppSandbox.html
[create-record]: https://developer.apple.com/library/ios/documentation/LanguagesUtilities/Conceptual/iTunesConnect_Guide/Chapters/CreatingiTunesConnectRecord.html
[submit-for-review]: https://developer.apple.com/library/ios/documentation/LanguagesUtilities/Conceptual/iTunesConnect_Guide/Chapters/SubmittingTheApp.html
[app-sandboxing]: https://developer.apple.com/app-sandboxing/
[ern-tutorial]: https://carouselapps.com/2015/12/15/legally-submit-app-apples-app-store-uses-encryption-obtain-ern/
[temporary-exception]: https://developer.apple.com/library/mac/documentation/Miscellaneous/Reference/EntitlementKeyReference/Chapters/AppSandboxTemporaryExceptionEntitlements.html
