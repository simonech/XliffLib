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
    <sch:title>Schematron rules for checking the constraints of the Metadata module against XLIFF Version &version;</sch:title>
    <sch:ns prefix="mda" uri="urn:oasis:names:tc:xliff:metadata:2.0"/>
    <sch:ns prefix="xlf" uri="urn:oasis:names:tc:xliff:document:2.0"/>
    
    <sch:pattern id="mda1">
        <sch:rule context=" mda:metaGroup[@id]">
            <sch:let name="id" value="@id"/>
            <sch:let name="fragid" value="concat('f=', ancestor::xlf:file/@id)"/>
            <sch:report diagnostics="fragid-report" see="&this-loc;/&name;-&stage;.html#meta_id"
                test="ancestor::mda:metadata[@id=$id] or count(ancestor::mda:metadata//mda:metaGroup[@id=$id])>1">
                id duplication found. The value of id attribute (<sch:value-of select="$id"/>) must be unique among all 'mda:metaGroup' elements and the ancestor 'mda:metedata' element within the scope of the module. 
            </sch:report>
        </sch:rule>
    </sch:pattern>
    
    <sch:diagnostics>
        <sch:diagnostic id="fragid-report">#<sch:value-of select="$fragid"/></sch:diagnostic>
    </sch:diagnostics>
</sch:schema>