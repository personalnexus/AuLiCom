# AuLiCom — Aula Light Commander

Aula Light Commander connects to a serial port to control [DMX512](https://en.wikipedia.org/wiki/DMX512) devices e.g. via a USB-DMX adapter with the FTDI chip.

I have written about the beginnings of this project on [my blog](https://personalnexus.wordpress.com/2023/06/24/rediscovering-the-joy-of-coding/).

AuLiCom is made up of the following component parts:

* **AuLiComLib**: encapsulates the logic of DMX communication, managing fixtures etc. indepdendant of any kind of user interface.

* **AuLiComCli** exposes a minimal command-line interface to set and get channel values.

	![AuLiComCli controlling an LED bar](/doc/CLI/AuLiComCli.png)

* **AuLiComGui** is a planned graphical user-interface, but which as of June 2023 has not been started yet.

* **AuLiComSim** an implementation of `IConnection` that shows a minimal graphical user-interface with the values received from the connected application to allow testing without actual DMX hardware.

	![LED simulator used in AuLiComCli](/doc/Simulator/AuLiComSim.png) 

* **AuLiComXL** is an Excel plugin to manage channel values, fixtures and scenes. As of June 2023, this is the most complete interface to AuLiCom's functionality.

	![AuLiComXL controlling an LED bar](/doc/Excel/AuLiComXL.png)

## Command Syntax

The command-line interface and the `AuLiComExecuteCommand` function in Excel let you control channel values using a simple command syntax.

Commands consist of two parts separated by the **@** character. The first part specifies the channels to adjust. The second part defines what kind of adjustment to make. Inputs are trimmed, so whitespace at the beginning, end and around separators is ignored.

The channel specification consists of one or more channel sections separated by the **+** character. Each channel section can be a:
 - **range of channel numbers** e.g. "6-11", where start and end are separated by the **-** character,
 - **channel number** e.g. "4"
 - **search sub-string** e.g. "red". The command executor will then search for this substring (ignoring case) in all registered fixtures' names, channel names, and aliases. All channels that match will be adjusted by the command.

The adjustment can be one of these:
- **absolute value** e.g. "5" to set all channels to the same value of 5%.
- **add percentage** e.g. "+10" to add 10 percentage points to all channel values. Values cannot go higher than 100%.
- **subtract percentage** e.g. "-15" to subtract 15 percentage points from all channel values. Values cannot go lower than 0%.
- **multiply** e.g. "*3" to triple channel values. Values are rounded, but cannot go higher than 100%.
- **divide** e.g. "/2" to halve channel values. Values are rounded, but cannot go lower than 0%.
- **empty** (i.e. there is nothing but whitespace after the **@**) to set channels to 100%.

### Examples

The following is an incomplete list of how different channel sections and adjustments can be combined to easily change many channels at once:

- ``1 @ +10``	increase channel 1 by 10 percentage points
- ``2+3 @ -15``	lower channels 2 and 3 by 15 percentage points
- ``4-7 @ *3``	triple the percentage values of channels 4 through 7
- ``8-9 + 10 @ /2``	halve the percentage values of channels 8 through 9 and 0
- ``11 @ 50``	set channel 11 to 50%
- ``12-15 @ 60``	set channels 11 through 15 to 60%
- ``16 + 19 @ 70``	set channels 16 and 19 to 70%
- ``20-25 + 27-30 @ 80``	set channels 20 through 25 and 27 through 30 to 80% 
- ``Red @ 90``	set channels on fixtures where channel name, fixture name or lias contains 'red' (ignoring case) to - 90%
- ``31-33 @``	set channels 31 through 33 to 100%


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

