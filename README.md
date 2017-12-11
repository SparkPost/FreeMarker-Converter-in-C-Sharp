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

<PRE>
More complex conversions are also possible.  The current version will operate on the following:
 - Basic substitutions
 
look for these to replace
FreeMarker support logical operators:
  Logical or: ||
  Logical and: &&
  Logical not: !

There are FLAGS, INTERPOLATONS, and COMMENTS to account for.
  FLAGS = <#action ... >
  COMMENTS=  <#-- comment -->
  INTERPOLATION = ${variable}

Samples of FLAGS are:
<#list ...>
<#if ...>
Note that user defined directives are not yet supported.  IE: <@directive ...> will fail
Samples of INTERPOLATION are:
  ${Recipient.lead.firstname}
  ${Gears.unsubscribe()}
  
 Watch out for character slicing which is not currently supporteed
 IE: ${Recipient.lead.firstname[0]}
 
 Also watch for logical operator, IE:
  IE: ${Recipient.lead.firstname[0]!""}
  ^^ in other words, Recipient firstname first character is NOT == ""
  
Turn "assignments into substitution vars. IE:  
  <#assign countrycontact=Recipient.contact.pv_landenid[0]!"" />
  BECOMES...
  "substitution_data" : {"countrycontact" :"Recipient.contact.pv_landenid"}
  
</PRE>
