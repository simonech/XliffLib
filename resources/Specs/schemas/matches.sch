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
    <sch:title>Schematron rules for checking the constraints of the Translation Candidate Annotation module against XLIFF Version &version;</sch:title>
    <sch:ns prefix="mtc" uri="urn:oasis:names:tc:xliff:matches:2.0"/>
    <sch:ns prefix="xlf" uri="urn:oasis:names:tc:xliff:document:2.0"/> 
    <sch:pattern id="K5MTC">
        <sch:title>The value [of id] must be unique among all 'data' id attribute values within the enclosing 'originalData' element</sch:title>
        <sch:rule context="xlf:data[ancestor::mtc:matches]">
            <sch:let name="id" value="current()/@id"/>
            <sch:let name="fragid" value="concat('f=',ancestor::xlf:file/@id)"/>
            <sch:report diagnostics="fragid-report" see="&this-loc;/&name;-&stage;.html#id"
                test="following-sibling::xlf:data[@id=$id]">
                id duplication found. The value '<sch:value-of select="$id"/>' is used more than once for 'data' elements 
                within the enclosing 'originalData' in the MTC module.
            </sch:report>
        </sch:rule>
    </sch:pattern>
    <sch:pattern id="K6MTC-S">
        <sch:title>Except for the above exception [target duplication], the value [of id attribute] must be unique among all of the above 
            [segment, ignorable, mkr, sm,pc, sc, ec, ph] within the enclosing 'revision' element</sch:title>
        <sch:rule context="xlf:source[ancestor::mtc:match]//xlf:*[@id]">
            <sch:let name="id" value="current()/@id"/>
            <sch:report see="&this-loc;/&name;-&stage;.html#id"
                test="count(ancestor::xlf:source//xlf:*[@id=$id])>1">
                id duplication found. The value '<sch:value-of select="$id"/>' is used more then once among inline and/or 'segmen'/'ignorable' 
                elements within the enclosing 'revision'.
            </sch:report>
        </sch:rule>
    </sch:pattern>
    <sch:pattern id="K6MTC-T">
        <sch:title>The inline elements enclosed by a 'target' element must use the duplicate id values of their corresponding inline elements enclosed within the sibling 'source' element if and only if those corresponding elements exist</sch:title>
        <sch:rule context="xlf:target[ancestor::mtc:match]//xlf:*[@id]">
            <sch:let name="id" value="@id"/>
            <sch:let name="file-id" value="ancestor::xlf:file/@id"/>
            <sch:let name="unit-id" value="ancestor::xlf:unit/@id"/>
            <sch:let name="counter" value="count(ancestor::mtc:match//xlf:*[@id=$id])"/>
            <!-- If the counter is 1, then the id is unique and therefore valid -->
            <!-- If the counter is 2 (duplication exsists), the duplication must be 
            in the sibling source, have the same name and contain same attributes-->
            <!-- If the counter is more than 2 or the duplication is not in the right 
            position, then an error will be reported -->
            <sch:assert see="&this-loc;/&name;-&stage;.html#id"
                test="($counter=1) or ($counter=2 and
                count(ancestor::mtc:match//xlf:source//xlf:*[@id=$id]
                [local-name()= local-name(current())])=1)">
                Invalid id used for element '<sch:name/>'. It must duplicate the 
                id of its corresponding element, enclosed within the 'source' 
                element or be unique in the scope of 'revision'.
            </sch:assert>
        </sch:rule>
    </sch:pattern>
    <sch:pattern id="mtc-F15">
        <sch:title>dataRef attribute must point to a data element within the same mtc:match</sch:title>
        <sch:rule context="xlf:*[@dataRef][ancestor::mtc:match]">
            <sch:assert test="ancestor::mtc:match/xlf:originalData/xlf:data[@id=current()/@dataRef]">
                'dataRef' attribute must point to a 'data' element within the same 'mtc:match' (allowed referencing pattern: dataRef="data-id").
            </sch:assert>
        </sch:rule>
    </sch:pattern>
    <sch:pattern id="mtc-F16">
        <sch:title>dataRefEnd attribute must point to a data element within the same mtc:match</sch:title>
        <sch:rule context="xlf:pc[@dataRefEnd][ancestor::mtc:match]">
            <sch:assert test="ancestor::mtc:match/xlf:originalData/xlf:data[@id=current()/@dataRefEnd]">
                'dataRefEnd' attribute must point to a 'data' element within the same 'mtc:match' (allowed referencing pattern: dataRefEnd="data-id").
            </sch:assert>
            <sch:assert test="@dataRefStart">
                'dataRefEnd' and 'dataRefStart' must be used in pair.
            </sch:assert>
        </sch:rule>
    </sch:pattern>
    <sch:pattern id="mtc-F17">
        <sch:title>dataRefStart attribute must point to a data element within the same mtc:match</sch:title>
        <sch:rule context="xlf:pc[@dataRefStart][ancestor::mtc:match]">
            <sch:assert test="ancestor::mtc:match/xlf:originalData/xlf:data[@id=current()/@dataRefStart]">
                'dataRefStart' attribute must point to a 'data' element within the same 'mtc:match' (allowed referencing pattern: dataRefStart="data-id").
            </sch:assert>
            <sch:assert test="@dataRefEnd">
                'dataRefStart' and 'dataRefEnd' must be used in pair.
            </sch:assert>
        </sch:rule>
    </sch:pattern>
    <sch:pattern id="mtc1">
        <sch:rule context="mtc:match[@id]">
            <sch:let name="id" value="@id"/>
            <sch:let name="fragid" value="concat('f=', ancestor::xlf:file/@id, '/u=', ancestor::xlf:unit/@id)"/>
            <sch:report diagnostics="fragid-report" see="&this-loc;/&name;-&stage;.html#candidates_id"
                test="following-sibling::mtc:match[@id=$id]">
                id duplication found. The value '<sch:value-of select="$id"/>' is used more than once for 'match' elements within the enclosing 'matches' element.
            </sch:report>
        </sch:rule>        
    </sch:pattern>
    <sch:pattern id="mtc2">
        <sch:rule context="mtc:match">
            <sch:let name="ref" value="@ref"/>
            <sch:let name="fragid" value="concat('f=', ancestor::xlf:file/@id, '/u=', ancestor::xlf:unit/@id)"/>
            <sch:assert diagnostics="fragid-report" see="&this-loc;/&name;-&stage;.html#candidates_ref"
                test="(not(contains($ref,'#')) and ancestor::xlf:unit//xlf:*[@id=$ref][ancestor-or-self::xlf:segment]) or 
                (contains($ref,'#') and ancestor::xlf:unit//xlf:*[@id=substring-after($ref,'#')][ancestor-or-self::xlf:segment])">
                'ref' attribute must point to a span of text within the same 'unit' (allowed referencing patterns: ref="#id" or ref="id").
            </sch:assert>
        </sch:rule>
    </sch:pattern>
    <sch:pattern id="mtc3">
        <sch:rule context="mtc:match[@subType]">
            <sch:let name="fragid" value="concat('f=', ancestor::xlf:file/@id, '/u=', ancestor::xlf:unit/@id)"/>
            <sch:assert diagnostics="fragid-report"  see="&this-loc;/&name;-&stage;.html#candidates_subtype"
                test="@type" >
                'subType' attribute is used, but the 'type attribute is missing.
            </sch:assert>
        </sch:rule>
    </sch:pattern>
    <sch:pattern id="mtc4">
        <sch:rule context="mtc:match//xlf:sc[@isolated='no' or not(@isolated)]">
            <sch:assert test="ancestor::mtc:match//xlf:ec[@startRef=current()/@id]">
                There must not be any unhandled orphan elements. The 'ec' element corresponding to this start code is missing in the same 'ctr:contentItem'.
            </sch:assert>
        </sch:rule>
    </sch:pattern>
    <sch:pattern id="mtc5">
        <sch:rule context="mtc:match//xlf:sm">
            <sch:assert test="ancestor::mtc:match//xlf:em[@startRef=current()/@id]">
                There must not be any unhandled orphan elements. The 'em' element corresponding to this start marker is missing in the same 'ctr:contentItem'.
            </sch:assert>
        </sch:rule>
    </sch:pattern>
    
    <sch:diagnostics>
        <sch:diagnostic id="fragid-report">#<sch:value-of select="$fragid"/></sch:diagnostic>
    </sch:diagnostics>
</sch:schema>