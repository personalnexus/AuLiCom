# AuLiCom — Aula Light Commander

Aula Light Commander connects to a serial port to control [DMX512](https://en.wikipedia.org/wiki/DMX512) devices e.g. via a USB-DMX adapter with the FTDI chip.

I have written about the beginnings of this project on [my blog](https://personalnexus.wordpress.com/2023/06/24/rediscovering-the-joy-of-coding/).

AuLiCom is made up of the following component parts:

* **AuLiComLib**: encapsulates the logic of DMX communication, managing fixtures etc. indepdendant of any kind of user interface.

* **AuLiComCli** exposes a minimal command-line interface to set and get channel values.

	![AuLiComCli controlling an LED bar](/doc/CLI/AuLiComCli.png)

* **AuLiComGui** is a planned graphical user-interface, but which as of June 2023 has not been started yet.

* **AuLiComSim** is a planned simulator that implements `IConnection` and shows a minimal graphical user-interface with the values received from the connected application to allow testing without actual DMX hardware.

* **AuLiComXL** is an Excel plugin to manage channel values, fixtures and scenes. As of June 2023, this is the most complete interface to AuLiCom's functionality.

	![AuLiComXL controlling an LED bar](/doc/Excel/AuLiComXL.png)

## Class Design

All functional modules are to be isolated as much as possible, allowing separate developers to work on each. Dependencies are managed through interfaces passed into the constructor. While a dependency injection framework is not currently used, this design should make it easy to retrofit one later. Also, most fields should be readonly reducing mutability as much as possible.

In the beginning DMX512 via serial port is the only protocol used, but at no point must there be a dependency on this one protocol. It should be possible to add something like [Art-Net](https://en.wikipedia.org/wiki/Art-Net) later on.

## Library Dependencies

AuLiCom has dependencies on the following libraries:

* **ExcelDna:** exposes user-defined functions and a COM server to Excel
* **FluentAssertions:** provides fluent syntax for assertions in unit tests
* **MSTest:** runs unit tests
* **Newtonsoft.Json:** reads JSON configuration files (I tried using System.Text.Json instead, but the migration is going to take more effort then I'm willing to invest right now)
* **System.IO.Ports:** connects to a USB-DMX adapter with the FTDI chip.
* **TestableIO.System.IO.Abstractions:** abstracts the file system, so loading files in unit test can be done purely in-memory without having any files on disk.

