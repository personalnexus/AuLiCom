# Class Design

All functional modules are to be isolated as much as possible, allowing separate developers to work on each. Dependencies are managed through interfaces passed into the constructor. While a dependency injection framework is not currently used, this design should make it easy to retrofit one later. Also, most fields should be readonly reducing mutability as much as possible.

In the beginning DMX512 via serial port is the only protocol used, but at no point must there be a dependency on this one protocol. It should be possible to add something like [Art-Net](https://en.wikipedia.org/wiki/Art-Net) later on.

# Library Dependencies

AuLiCom has dependencies on the following libraries:

* **ExcelDna:** exposes user-defined functions and a COM server to Excel
* **FluentAssertions:** provides fluent syntax for assertions in unit tests
* **MSTest:** runs unit tests
* **Newtonsoft.Json:** reads JSON configuration files (I tried using System.Text.Json instead, but the migration is going to take more effort then I'm willing to invest right now)
* **System.IO.Ports:** connects to a USB-DMX adapter with the FTDI chip.
* **TestableIO.System.IO.Abstractions:** abstracts the file system, so loading files in unit test can be done purely in-memory without having any files on disk.

