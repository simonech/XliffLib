<?xml version="1.0" encoding="utf-8"?>
<!DOCTYPE schematron [
<!-- Entities for XLIFF V2.x publishing.................................... -->
<!--copy all of these to all *.sch files and also to the Oxygen framework for validating Docbook 4.5 if you use Oxygen -->
<!ENTITY name "xliff-core-v2.1">
<!ENTITY pversion "2.0">
<!ENTITY version "2.1">
<!ENTITY bschversion "2.0">
<!ENTITY cschversion "2.1">

<!ENTITY stage "csprd04">
<!ENTITY pstage "csprd03">
<!ENTITY standard "Commitee Specification Draft 04 / Public Review Draft 04">
<!-- <!ENTITY standard "Working Draft 03">-->

<!ENTITY this-loc "http://docs.oasis-open.org/xliff/xliff-core/v&version;/&stage;">
<!ENTITY previous-loc "http://docs.oasis-open.org/xliff/xliff-core/v&version;/&pstage;">
<!ENTITY latest-loc "http://docs.oasis-open.org/xliff/xliff-core/v&version;">
<!ENTITY pubdate "02 May &pubyear;">
<!ENTITY pubyear "2017">
<!ENTITY releaseinfo "Standards Track Work Product">
<!-- End of XLIFF V2.x publishing entities -->
]>


<sch:schema xmlns:sch="http://purl.oclc.org/dsdl/schematron" queryBinding='xslt2'>
    <sch:title>Schematron rules for checking the constraints of the Format Style module against XLIFF Version &version;</sch:title>
    <sch:ns prefix="fs" uri="urn:oasis:names:tc:xliff:fs:2.0"/>
    <sch:ns prefix="xlf" uri="urn:oasis:names:tc:xliff:document:2.0"/>
    
    <sch:pattern id="fs1">
        <sch:rule context="xlf:ec[@fs:fs]">
            <sch:let name="fragid" value="concat('f=', ancestor::xlf:file/@id, '/u=', ancestor::xlf:unit/@id)"/>
            <sch:assert diagnostics="fragid-report" see="&this-loc;/&name;-&stage;.html#fs"
                test="current()/@isolate='yes'">
                The fs:fs attribute is used, but the isolated attribute is not set to 'yes'.
            </sch:assert>
        </sch:rule>
    </sch:pattern>
    <sch:pattern id="fs2">
        <sch:rule context="xlf:ec[@fs:subFs]">
            <sch:let name="fragid" value="concat('f=', ancestor::xlf:file/@id)"/>
            <sch:assert diagnostics="fragid-report" see="&this-loc;/&name;-&stage;.html#subFs"
                test="current()/@isolated='yes'">
                The fs:subFs attribute is used, but the isolated attribute is not set to 'yes'.
            </sch:assert>
        </sch:rule>
    </sch:pattern>
    <sch:pattern id="fs3">
        <sch:rule context="xlf:*[@fs:subFs]">
            <sch:let name="fragid" value="concat('f=', ancestor::xlf:file/@id)"/>
            <sch:assert diagnostics="fragid-report" see="&this-loc;/&name;-&stage;.html#subFs"
                test="current()/@fs:fs">
               The fs:subFs attribute is used, but fs:fs is missing.
            </sch:assert>
        </sch:rule>
    </sch:pattern>
    
    
    
  
        <sch:diagnostics>
            <sch:diagnostic id="fragid-report">#<sch:value-of select="$fragid"/></sch:diagnostic>
        </sch:diagnostics>
</sch:schema>