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
 


 /* look for these to replace
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
  
  
<#assign countrycontact=Recipient.contact.pv_landenid[0]!"" />
<#assign countryaccount=Recipient.account.pv_landcodeid[0]!"" />
<#assign countrylead=Recipient.lead.pv_landcodeid[0]!"" />
<#assign utmnamecontact=Recipient.contact.kl_verkoper_trucks_id.systemuser.kl_utmname[0]!"" />
<#assign utmnameaccount=Recipient.account.kl_verkoper_trucks_id.systemuser.kl_utmname[0]!""/>
<#assign utmnamelead=Recipient.lead.kl_verkoper_trucks_id.systemuser.kl_utmname[0]!"" />
${countrycontact}
${countrylead}
${countryaccount}
${utmnamecontact}
${utmnameaccount}
${utmnamelead}
${Recipient.EncryptedEmail}
<#assign firstnamecontact=Recipient.contact.kl_verkoper_trucks_id.systemuser.firstname[0]!""/>\
<#assign firstnameaccount=Recipient.account.kl_verkoper_trucks_id.systemuser.firstname[0]!""/>
<#assign firstnamelead=Recipient.lead.kl_verkoper_trucks_id.systemuser.firstname[0]!""/>
<#assign photourlcontact=Recipient.contact.kl_verkoper_trucks_id.systemuser.photourl[0]!""/>
<#assign photourlaccount=Recipient.account.kl_verkoper_trucks_id.systemuser.photourl[0]!""/>
<#assign photourllead=Recipient.lead.kl_verkoper_trucks_id.systemuser.photourl[0]!""/>
<#assign emailcontact=Recipient.contact.kl_verkoper_trucks_id.systemuser.internalemailaddress[0]!""/>
<#assign emailaccount=Recipient.account.kl_verkoper_trucks_id.systemuser.internalemailaddress[0]!""/>
<#assign emaillead=Recipient.lead.kl_verkoper_trucks_id.systemuser.internalemailaddress[0]!""/>
<#assign phonenumbercontact=Recipient.contact.kl_verkoper_trucks_id.systemuser.address1_telephone1[0]!""/>
<#assign phonenumberaccount=Recipient.account.kl_verkoper_trucks_id.systemuser.address1_telephone1[0]!""/>
<#assign phonenumberlead=Recipient.lead.kl_verkoper_trucks_id.systemuser.address1_telephone1[0]!""/>
<#assign skypecontact=Recipient.contact.kl_verkoper_trucks_id.systemuser.kl_skype[0]!""/>
<#assign skypeaccount=Recipient.account.kl_verkoper_trucks_id.systemuser.kl_skype[0]!""/>
<#assign skypelead=Recipient.lead.kl_verkoper_trucks_id.systemuser.kl_skype[0]!""/>
<#assign twittercontact=Recipient.contact.kl_verkoper_trucks_id.systemuser.kl_twitter[0]!""/>
<#assign twitteraccount=Recipient.account.kl_verkoper_trucks_id.systemuser.kl_twitter[0]!""/>
<#assign twitterlead=Recipient.lead.kl_verkoper_trucks_id.systemuser.kl_twitter[0]!""/>
<#assign linkedincontact=Recipient.contact.kl_verkoper_trucks_id.systemuser.kl_linkedin[0]!""/>
<#assign linkedinaccount=Recipient.account.kl_verkoper_trucks_id.systemuser.kl_linkedin[0]!""/>
<#assign linkedinlead=Recipient.lead.kl_verkoper_trucks_id.systemuser.kl_linkedin[0]!""/>
<#assign facebookcontact=Recipient.contact.kl_verkoper_trucks_id.systemuser.kl_facebook[0]!""/>
<#assign facebookaccount=Recipient.account.kl_verkoper_trucks_id.systemuser.kl_facebook[0]!""/>
<#assign facebooklead=Recipient.lead.kl_verkoper_trucks_id.systemuser.kl_facebook[0]!""/> 
*/
  
