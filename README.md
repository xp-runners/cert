Certificate management
======================
[![Build Status on TravisCI](https://secure.travis-ci.org/xp-runners/cert.svg)](http://travis-ci.org/xp-runners/cert)
[![BSD License](https://raw.githubusercontent.com/xp-framework/web/master/static/licence-bsd.png)](https://github.com/xp-runners/cert/blob/master/LICENSE.md)


This tool takes care of creating a standardized `ca-bundle.crt` file for various environments. Born to solve [this issue](https://github.com/xp-framework/core/issues/150).

Usage
-----
Update the `ca-bundle.crt` file in the current directory:

```sh
vagrant@virtualbox ~/.xp
$ cert up
@unix (detected)
Updating certificates

> Linked ca-bundle.crt -> /etc/ssl/certs/ca-certificates.crt
  173 certificates

Done, /home/vagrant/.xp/ca-bundle.crt updated
```

Depending on the underlying operating system and runtime environment, the action to create the file may be different. For details, see below:

Windows
-------
On Windows, the system certificate store is exported. *This needs to be rerun every time Microsoft updates their OS.*

Mac OS X
--------
On Mac OS X, the system keychain is exported. *This needs to be rerun every time Apple updates their OS.*

Cygwin
------
If a Cygwin environment is present, a symlink to `/etc/pki/tls/certs/ca-bundle.crt` is created. This does not need to be re-run except if Cygwin's vendors decide to change this path.

Linux/UNIX systems
------------------
A symlink is created to whichever of the following can be found first:

| Path                                     | Typical for               |
| ---------------------------------------- | ------------------------- |
| `/etc/ssl/certs/ca-certificates.crt`     | Debian/Ubuntu/Gentoo etc. |
| `/etc/pki/tls/certs/ca-bundle.crt`       | Fedora/RHEL               |
| `/etc/ssl/ca-bundle.pem`                 | OpenSUSE                  |
| `/etc/pki/tls/cacert.pem`                | OpenELEC                  |
| `/usr/local/share/certs/ca-root-nss.crt` | FreeBSD/DragonFly         |
| `/etc/ssl/cert.pem`                      | OpenBSD                   |
| `/etc/openssl/certs/ca-certificates.crt` | NetBSD                    |

This only needs to be rerun if the OS' vendor decides to change this path.
