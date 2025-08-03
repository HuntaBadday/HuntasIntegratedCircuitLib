# HuntasIntegratedCircuitLib

HuntasICLib is a library that provides building blocks for other components to use. This library provides classes that handles timers, serial, CPUs, and other circuits to use in larger projects.

## Installation
To install, copy the ``HuntasICLib`` folder to Logic World's ``GameData`` folder.

## Available classes
## CPU
### TSC-LWC33
Documentation: https://huntabadday.com/docs/TSC-LWC33.pdf

#### Functions:
LogicUpdate(): Update CPU's internal logic

serializeCPUState() -> byte[]: Serialize the CPU's internal state to byte[]

deserializeCPUState(byte[]): Set the CPU's internal state to data from a byte[]

#### I/O
For I/O reference, see code comments in CPU IO section (Line 139) ``HuntasICLib/src/server/CPU/LWC33.cs``

### MOS 6502
Documentation: Search the internet

#### Functions:
LogicUpdate(): Update CPU's internal logic

serializeCPUState() -> byte[]: Serialize the CPU's internal state to byte[]

deserializeCPUState(byte[]): Set the CPU's internal state to data from a byte[]

#### I/O
For I/O reference, see code comments in CPU IO section (Line 263) ``HuntasICLib/src/server/CPU/MOS6502.cs``

## Buffers
There are two types of buffers, FIFO (First In First Out) and LIFO (Last In First Out). Each has an 8 bit and a 16 bit variation. See ``HuntasICLib/src/server/Memory/`` reference.

## Serial transmitters/receivers
See ``HuntasICLib/src/server/Serial``

## Timers
### TSC 6530
``HuntasICLib.Timer.TSC6530``

The TSC6530 is essentially just the timers from a MOS6526. See the source code for I/O information. Documentation can be found in the same directory as the IC.