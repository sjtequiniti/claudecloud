Claude Cloud
============

Deploy an APL64-powered Azure Durable Function with help from Claude Code.


## Background

Superval is an APL+Win desktop application recently migrated to APL64
from [APL2000](http://www.apl2000.com/). It allows actuaries to
evaluate the liabilities of a defined-benefit pension scheme under
different scenarios.

It consists of an extensive GUI based on APL's `⎕WI` primitive, which
is used to prepare data files for calculation, which produces
reports. The data for a calculation consists of facts about the
scheme and its members and parameters that describe the scenario.

The calculation process entails evaluating each member record. Schemes
with many members can take hours to evaluate. The evaluations are
independent, so the whole calculation is a prime candidate for
parallel processing.

The data files for a large calculation are <500MB.


## Goal

The project goal is to cut calculation time significantly by deploying
the calculation as an Azure Durable Function, exploiting its capacity
for horizontal scaling on the fan-out/fan-in model, and creating
processing resources on the Azure cloud platform for the duration of
the calculation.

The project expects on-demand horizontal scaling to avoid 

-   configuring and paying for permanent cloud computing resource
	that is likely to be idle for long periods
-   developing orchestration software to manage the fan-out and fan-in 

At some future time we might migrate the application to Dyalog APL.
The calculation would be easier to migrate than the GUI, so it would
be advantageous then if the calculation were already running
separately on a cloud platform. A secondary goal of the project is to
confirm that a Dyalog APL process can be deployed as an Azure
Function or an Azure Durable Function.


## Technical risks

1.  We are unfamiliar with the Azure cloud; the project will be a learning experience.
2.  APL is not one of the programming languages Azure supports; we cannot upload
	APL source code to be run, but must provide a binary executable, which depends on
	the .Net framework.
3.  We know of no examples of deploying APL processes to a cloud platform.
4.  We have little familiarity with Docker, especially on a Windows platform.
5.  We have little familiarity with APL64's new cross-platform component.


### APL64

> An APL64 cross-platform component (CPC) is a Nuget package containing
> a .Net Core assembly which exposes one or more APL64
> programmer-defined ‘public’ functions in the workspace. This package
> may be used as part of a containing .Net application to support the
> APL business rules or algorithms of the application. Any .Net
> programming language may be used to access the APL64 cross-platform
> component in a hardware and operating system environment which
> supports Microsoft .Net Standard v2.1.

Preliminary trials have produced CPCs of about 300MB.


### Dyalog APL

Dyalog APL can export a binary runtime. Including a .Net framework means it weighs about 300MB.


## Project parts and phases

To manage the technical risks we can proceed in small steps.

Azure (Durable) Functions support six popular scripting languages. Even
with APL doing the core processing, one of these languages will be
required for at least the top level of a Function. Of these, Node is
the most familiar.

The first step in each part is to get an endpoint working using one of
the languages Azure Functions support. In this step, all the Function
code is in the selected language, and can be uploaded to the platform
as source code.

However, both APL64 and Dyalog runtime binaries depend on the .Net
framework.

Currently these binaries incorporate .Net and weigh about 300MB.

Azure Functions offers an execution environment that includes .Net. That
might make it possible to lighten the APL executables by 90%, if the
APL64 and Dyalog APL interpreters can export executables that depend
instead on .Net in the execution environment.

An Azure Functions runtime environment that includes .Net might require
C# as the scripting language.


### Part 1: Azure Function (AF)

Part 1 must be completed before starting Part 2.

The goal of this part is to confirm that we can deploy an APL64 or
Dyalog APL process as an Azure Function.

For proof of concept we can use as POST payload a CSV file representing
a matrix of random integers, for which column sums are to be
calculated. The AF result will be a JSON list of column sums.

1.  Create an Azure Function using the scripting language.
2.  Replace the script with an APL64 executable
3.  Replace the script with a Dyalog APL executable

Tests will confirm the JSON list in the result is the correct column sums.

### Part 2: Azure Durable Function (ADF)

An AF is designed for a fast response and its limits on execution time
preclude its use for a Superval calculation.
For this we need an ADF, which is allowed longer to complete its work.

Our ADF workflow must begin with the upload of a large file to be worked on
by multiple Worker processes.

For proof of concept we can use the same CSV file as for the AF.

1.  Create an Azure Durable Function using the scripting language.
2.  Replace the script with an APL64 executable
3.  Replace the script with a Dyalog APL executable

Tests will confirm the JSON list in the result is the correct column sums.


## Questions

**The project is achievable only if (1) or (2) is Yes.**

### Open

1.  Does an Azure runtime with .Net installed restrict
	the choice of scripting language?
2.  A Superval calculation takes as input a ZIP of <500MB. The Orchestration
	process must start Worker processes with read access to the ZIP contents.
	How should this be done? Azure File or Azure Blob?
3.	Does invoking the ADF require two steps: (a) upload the input file,
	(b) start the ADF? Or can the upload invoke the ADF?
4.  When the ADF completes can the Orchestrator process delete the input file,
	or must this be initiated outside the Azure cloud?

### Answered or decided

1.  Can either APL64 or Dyalog APL export an executable that depends on
	.Net in its execution environment?
	**No** (as of 2025). Dyalog is considering this for a future release.

2.  Can either an AF or an ADF accept a 300MB executable, either uploaded
	as part of the function definition, or imported from a NuGet repository?
	**Yes** (applies to both AF and ADF; Durable Functions is an extension
	running on the same hosting infrastructure). Max deployment package is
	1 GB. Temp storage limits by plan:

	| Plan                | Temp Storage | Scales to Zero | 300MB viable? |
	|---------------------|--------------|----------------|---------------|
	| Consumption         | 500 MB       | Yes            | Tight         |
	| Flex Consumption    | 800 MB       | Yes            | Yes           |
	| Consumption (Linux) | 1.5 GB       | Yes            | Yes           |
	| Premium             | 21-140 GB    | No             | Yes           |
	| Dedicated           | 11-140 GB    | No             | Yes           |

	**Recommendation**: Use Flex Consumption plan. It has sufficient temp
	storage (800 MB) for a 300 MB package, scales to zero when idle, and
	uses pay-per-execution billing. Premium and Dedicated plans incur costs
	even when idle. Deploy via custom handler.

	Sources (retrieved 2026-02-01):
	- https://learn.microsoft.com/en-us/azure/azure-functions/run-functions-from-deployment-package
	- https://learn.microsoft.com/en-us/azure/azure-functions/functions-custom-handlers
	- https://learn.microsoft.com/en-us/azure/azure-functions/functions-scale
	- https://learn.microsoft.com/en-us/azure/azure-functions/flex-consumption-plan
	- https://azure.microsoft.com/en-us/pricing/details/functions/

