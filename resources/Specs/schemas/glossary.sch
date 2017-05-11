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

<sch:schema xmlns:sch="http://purl.oclc.org/dsdl/schematron" queryBinding='xslt2' >
    <sch:title>Schematron rules for checking the constraints of the Glossary module against XLIFF Vesrion &version; spec</sch:title>
    <sch:ns prefix="gls" uri="urn:oasis:names:tc:xliff:glossary:2.0"/>
    <sch:ns prefix="xlf" uri="urn:oasis:names:tc:xliff:document:2.0"/>    
    <sch:pattern id="gls1">
        <sch:rule context="gls:glossEntry[@id] | gls:translation[@id]">
            <sch:let name="id" value="@id"/>
            <sch:let name="fragid" value="concat('f=', ancestor::xlf:file/@id)"/>
            <sch:report diagnostics="fragid-report" see="&this-loc;/&name;-&stage;.html#gls_id"
                test="count(ancestor::gls:glossary//gls:glossEntry[@id=$id] | ancestor::gls:glossary//gls:translation[@id=$id])>1">
                id duplication found among 'gls:glossEntry' and/or 'gls:translation' elements within the Glossary Module.
            </sch:report>
        </sch:rule>
    </sch:pattern>
    <sch:pattern id="gls2">
        <sch:rule context="gls:glossEntry">
            <sch:let name="fragid" value="concat('f=', ancestor::xlf:file/@id)"/>
            <sch:assert diagnostics="fragid-report"  see="&this-loc;/&name;-&stage;.html#glossentry" 
                test="child::gls:translation or child::gls:definition">
                Incomplete 'gls:glossEntry' element.
            </sch:assert>
        </sch:rule>
    </sch:pattern>
    <sch:pattern id="gls3">
        <sch:rule context="gls:glossEntry[@ref] | gls:translation[@ref]">
            <sch:let name="ref" value="@ref"/>
            <sch:let name="fragid" value="concat('f=', ancestor::xlf:file/@id, '/u=', ancestor::xlf:unit/@id)"/>
            <sch:assert diagnostics="fragid-report" see="&this-loc;/&name;-&stage;.html#gls_ref"
                test="(not(contains($ref,'#')) and ancestor::xlf:unit//xlf:*[@id=$ref][ancestor-or-self::xlf:segment]) or 
                (contains($ref,'#') and ancestor::xlf:unit//xlf:*[@id=substring-after($ref,'#')][ancestor-or-self::xlf:segment])">
                'ref' attribute must point to a span of text within the same 'unit' (allowed referencing patterns: ref="#id" or ref="id").
            </sch:assert>
        </sch:rule>
    </sch:pattern>
    
    <sch:diagnostics>
        <sch:diagnostic id="fragid-report">#<sch:value-of select="$fragid"/></sch:diagnostic>
    </sch:diagnostics>
</sch:schema>
