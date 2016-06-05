Certificate management
======================
[![Build Status on TravisCI](https://secure.travis-ci.org/xp-runners/cert.svg)](http://travis-ci.org/xp-runners/cert)
[![BSD License](https://raw.githubusercontent.com/xp-framework/web/master/static/licence-bsd.png)](https://github.com/xp-runners/cert/blob/master/LICENSE.md)


This tool takes care of creating a standardized `ca-bundle.crt` file for various environments. Born to solve [this issue](https://github.com/xp-framework/core/issues/150).

Windows
-------
On Windows, the system certificate store is exported. *This needs to be rerun every time Microsoft updates their OS.*

```sh
C:\tools\cygwin\home\Timm\devel\runners\cert>cert.exe up
@windows (detected)
Updating certificates

> From Root: [.....................................................]
  53 certificates

> From AuthRoot: [............................................]
  44 certificates

> From CertificateAuthority: [..........................]
  26 certificates

Done, C:\tools\cygwin\home\Timm\devel\runners\cert\ca-bundle.crt updated
```

Mac OS X
--------
On Mac OS X, the system keychain is exported. *This needs to be rerun every time Apple updates their OS.*

```sh
The-Mac:cert thekid$ mono cert.exe up
@macosx (detected)
Updating certificates

> From /System/Library/Keychains/SystemRootCertificates.keychain: [.....]
  211 certificates

Done, /Users/thekid/devel/cert/ca-bundle.crt updated
```

Cygwin
------
If a Cygwin environment is present, a symlink to `/etc/pki/tls/certs/ca-bundle.crt` is created. This does not need to be re-run except if Cygwin's vendors decide to change this path.

```sh
Timm@slate ~/devel/runners/cert [master]
$ ./cert.exe up
@cygwin (detected)
Updating certificates

> Linked ca-bundle.crt -> /etc/pki/tls/certs/ca-bundle.crt
  169 certificates

Done, C:\tools\cygwin\home\Timm\devel\runners\cert\ca-bundle.crt updated
```

Linux systems
-------------
A symlink is created to whichever of the following can be found first:

| Path                                 | Typical for               |
| ------------------------------------ | ------------------------- |
| `/etc/ssl/certs/ca-certificates.crt` | Debian/Ubuntu/Gentoo etc. |
| `/etc/pki/tls/certs/ca-bundle.crt`   | Fedora/RHEL               |
| `/etc/ssl/ca-bundle.pem`             | OpenSUSE                  |
| `/etc/pki/tls/cacert.pem`            | OpenELEC                  |

This only needs to be rerun if the OS' vendor decides to change this path.

```sh
vagrant@vagrant-ubuntu-vivid-64:~/.xp$ /devel/runners/cert/cert.exe up
@unix (detected)
Updating certificates

> Linked ca-bundle.crt -> /etc/ssl/certs/ca-certificates.crt
  173 certificates

Done, /home/vagrant/.xp/ca-bundle.crt updated
```