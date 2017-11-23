# FreeMarker-Converter-in-C#

FreeMarker template converter utility (C# version)

This is a utility to convert FreeMarker templates to SparkPost templates.
* FreeMarker home: http://freemarker.org/ 
* FreeMarker Manual: http://freemarker.org/docs/index.html

== Installation ==
1) Install Git if it is not already installed
2) Clone this repo
3) Place FreeMarker templates for conversion into the INBOX folder
4) Execute the program.cs script 
5) Collect the converted templates from the OUTBOX folder


== Usage ==

In the sample.fm file inside INBOX, there are two simple substitutions.

${Recipient.lead.firstname[0]!""}  ,  ${Gears.unsubscribe()}

When converted to SparkPost template placeholders, they become

{{Recipient.lead.firstname}}  ,  {{Gears.unsubscribe}}

More complex conversions are also possible.  The current version will operate on the following:
 - Basic substitutions
 


