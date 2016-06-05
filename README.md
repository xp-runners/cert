Certificate management
======================
[![Build Status on TravisCI](https://secure.travis-ci.org/xp-runners/cert.svg)](http://travis-ci.org/xp-runners/cert)
[![BSD License](https://raw.githubusercontent.com/xp-framework/web/master/static/licence-bsd.png)](https://github.com/xp-runners/cert/blob/master/LICENCE.md)


This tool takes care of creating a standardized `ca-bundle.crt` file for various environments.

Windows
-------
On Windows, the system certificate store is exported. This needs to be rerun if Microsoft updates their OS.

Mac OS X
--------
On Mac OS X, the system keychain is exported. This needs to be rerun if Apple updates their OS.

Cygwin
------
If a Cygwin environment is present, a symlink to `/etc/pki/tls/certs/ca-bundle.crt` is created.

Linux systems
-------------
A symlink is created to whichever of the following can be found first:

| Path                                | Typical for               |
| ----------------------------------- | ------------------------- |
| /etc/ssl/certs/ca-certificates.crt  | Debian/Ubuntu/Gentoo etc. |
| /etc/pki/tls/certs/ca-bundle.crt    | Fedora/RHEL               |
| /etc/ssl/ca-bundle.pem"             | OpenSUSE                  |
| /etc/pki/tls/cacert.pem             | OpenELEC                  |
