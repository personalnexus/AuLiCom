# AuLiCom — Aula Light Commander

Aula Light Commander connects to a serial port to control [DMX512](https://en.wikipedia.org/wiki/DMX512) devices for example via a USB-DMX adapter with the FTDI chip.

AuLiCom is made up of the following component parts.

## AuLiComLib

AuLiComLib encapsulates the logic of DMX communication, managing fixtures, scenes etc. indepdendent of any kind of user interface.

## AuLiComCli

AuLiComCli exposes a minimal command-line interface to set and get channel values by calling AuLiComLib.

![AuLiComCli controlling an LED bar](/doc/CLI/AuLiComCli.png)

## AuLiComGui

AuLiComGui is a planned graphical user-interface on top of AuLiComLib, but which as of June 2023 has not been started yet.

## AuLiComSim

AuLiComSim is a library containing an implementation of `IConnection` that shows a minimal graphical user-interface (based on WPF) with the values received from AuLiComCli to allow testing without actual DMX hardware.

![LED simulator used in AuLiComCli](/doc/Simulator/AuLiComSim.png) 

## AuLiComXL

AuLiComXL is an Excel plugin to manage channel values, fixtures and scenes. The plugin exposes formulas to be used in a spreadsheet as well as a COM server to be used from VBA.

The following screenshots from [This Excel spreadsheet](/doc/Excel/TestWithComServer.xlsm) show what AuLiComXL can do. As of July 2023, this is the most complete interface to AuLiCom's functionality.

![AuLiComXL showing channels of an LED bar](/doc/Excel/Excel-Channels.png)

![AuLiComXL switching between pre-defined scenes](/doc/Excel/Excel-Scenes.png)

![AuLiComXL definition of fixtures](/doc/Excel/Excel-Fixtures.png)

# Further Reading

 * [Design](doc/Design.md)
 * [User Manual](doc/Manual.md)
 * [Origin Story on my blog](https://personalnexus.wordpress.com/2023/06/24/rediscovering-the-joy-of-coding/)

