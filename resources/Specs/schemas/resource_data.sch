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
    <sch:title>Schematron rules for checking the constraints of the Resource Data module against XLIFF Version &version;</sch:title>
    <sch:ns prefix="res" uri="urn:oasis:names:tc:xliff:resourcedata:2.0"/>
    <sch:ns prefix="xlf" uri="urn:oasis:names:tc:xliff:document:2.0"/>
    <sch:pattern id="res1">
        <sch:rule context="res:resourceItemRef[@id] | res:resourceItem[@id]">
            <sch:let name="id" value="@id"/>
            <sch:let name="fragid" value="concat('f=', ancestor::xlf:file/@id)"/>
            <sch:report diagnostics="fragid-report" see="&this-loc;/&name;-&stage;.html#res_resourceItemRef"
                test="count(ancestor::res:resourceData//res:resourceItem[@id=$id] | ancestor::res:resourceData//res:resourceItemRef[@id=$id])>1">
                id duplication found among 'res:resourceItemRef' and/or 'res:resourceItem' elements within the enclosing 'resourceData' element. 
            </sch:report>
        </sch:rule>
    </sch:pattern>
    <sch:pattern id="res2">
        <sch:rule context="res:resourceItem[not(@mimeType)]">
            <sch:let name="fragid" value="concat('f=', ancestor::xlf:file/@id)"/>
            <sch:assert diagnostics="fragid-report" see="&this-loc;/&name;-&stage;.html#res_resourceItem"
                test="./res:source/* and ./res:target/*">
                The children 'res:source' and/or 'res:target' elements are empty, but the mimeType attribute is missing.
            </sch:assert>
        </sch:rule>
    </sch:pattern>
    <sch:pattern id="res3">
        <sch:rule context="res:source">
            <sch:let name="fragid" value="concat('f=',ancestor::xlf:file/@id)"/>
            <sch:report diagnostics="fragid-report" see="&this-loc;/&name;-&stage;.html#res_source"
                test="not(@href) and not(child::node())">
                'res:source' element must not be empty when the 'href' attribute is missing.
            </sch:report>
            <sch:report diagnostics="fragid-report" see="&this-loc;/&name;-&stage;.html#res_source"
                test="@href and  child::node()">
                'res:source' element must be empty when containing 'href' attribute.
            </sch:report>
        </sch:rule>
    </sch:pattern>
    <sch:pattern id="res4">
        <sch:rule context="res:source[@xml:lang]">
            <sch:let name="fragid" value="concat('f=',ancestor::xlf:file/@id)"/>
            <sch:let name="srcLang" value="/xlf:xliff/@srcLang"/>
            <sch:assert diagnostics="fragid-report" see="&this-loc;/&name;-&stage;.html#res_source"
                test="lang($srcLang)">
                xml:lang attribute of 'res:source' element and 'srcLang' of enclosing 'xliff' are not matching.
            </sch:assert>
        </sch:rule>
    </sch:pattern>
    <sch:pattern id="res5">
        <sch:rule context="res:target">
            <sch:let name="fragid" value="concat('f=',ancestor::xlf:file/@id)"/>
            <sch:report diagnostics="fragid-report" see="&this-loc;/&name;-&stage;.html#res_target"
                test="not(@href) and not(child::node())">
                'res:target' element must not be empty when the 'href' attribute is missing.
            </sch:report>
            <sch:report diagnostics="fragid-report" see="&this-loc;/&name;-&stage;.html#res_target"
                test="@href and  child::node()">
                'res:target' element must be empty when containing 'href' attribute.
            </sch:report>
        </sch:rule>
    </sch:pattern>
    <sch:pattern id="res6">
        <sch:rule context="res:target[@xml:lang]">
            <sch:let name="fragid" value="concat('f=',ancestor::xlf:file/@id)"/>
            <sch:let name="trgLang" value="ancestor::xlf:xliff/@trgLang"/>
            <sch:assert diagnostics="fragid-report"  see="&this-loc;/&name;-&stage;.html#res_target"
                test="lang($trgLang)">
                xml:lang attribute of 'res:target' element and 'trgLang' of enclosing 'xliff' are not matching.
            </sch:assert>
        </sch:rule>
    </sch:pattern>
    <sch:pattern id="res7">
        <sch:rule context="res:resourceItemRef">
            <sch:let name="ref" value="@ref"/>
            <sch:let name="fragid" value="concat('f=',ancestor::xlf:file/@id)"/>
            <sch:assert diagnostics="fragid-report" see="&this-loc;/&name;-&stage;.html#res_ref"
                test="ancestor::xlf:file//res:resourceData/res:resourceItem[@id=$ref]">
                The 'res:resourceItem' element, to which the 'res:resourceItemRef' is pointing was not found at the file level (allowed referencing pattern: ref="id").
            </sch:assert>
        </sch:rule>
    </sch:pattern>
    <sch:diagnostics>
        <sch:diagnostic id="fragid-report">#<sch:value-of select="$fragid"/></sch:diagnostic>
    </sch:diagnostics>


</sch:schema>