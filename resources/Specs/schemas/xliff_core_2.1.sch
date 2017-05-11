<?xml version="1.0" encoding="UTF-8"?>
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
<sch:schema xmlns:sch="http://purl.oclc.org/dsdl/schematron" queryBinding="xslt2">
    <sch:title>Schematron Rules for Advanced Validation of XLIFF &version; Core constraints</sch:title>
    <!-- Namespace Declarations -->
    <sch:ns uri="urn:oasis:names:tc:xliff:document:2.0" prefix="xlf"/>
    <sch:ns uri="urn:oasis:names:tc:xliff:matches:2.0" prefix="mtc"/>
    <sch:ns uri="urn:oasis:names:tc:xliff:glossary:2.0" prefix="gls"/>
    <sch:ns uri="urn:oasis:names:tc:xliff:fs:2.0" prefix="fs"/>
    <sch:ns uri="urn:oasis:names:tc:xliff:metadata:2.0" prefix="mda"/>
    <sch:ns uri="urn:oasis:names:tc:xliff:resourcedata:2.0" prefix="res"/>
    <sch:ns uri="urn:oasis:names:tc:xliff:changetracking:2.1" prefix="ctr"/>
    <sch:ns uri="urn:oasis:names:tc:xliff:sizerestriction:2.0" prefix="slr"/>
    <sch:ns uri="urn:oasis:names:tc:xliff:validation:2.0" prefix="val"/>
    <sch:ns uri="http://www.w3.org/2005/11/its" prefix="its"/>
    <sch:ns uri="urn:oasis:names:tc:xliff:itsm:2.1" prefix="itsm"/>
    <!-- End of Namespace Declarations -->
    
    <!-- Rules for primary keys -->
    <sch:pattern id="K1">
        <sch:title>The value [of id] must be unique among all 'file' id attribute values within the enclosing 'xliff' element</sch:title>
        <sch:rule context="xlf:file">
            <sch:let name="id" value="current()/@id"/>
            <sch:let name="fragid" value="concat('f=',$id)"/>
            <sch:report diagnostics="fragid-report" see="&this-loc;/&name;-&stage;.html#id"
                test="following-sibling::xlf:file[@id=$id]">
                id duplication found. The value '<sch:value-of select="$id"/>' is used more than once for 'file' elements 
                within the enclosing 'xliff'.
            </sch:report>
        </sch:rule>
    </sch:pattern>
    <sch:pattern id="K2">
        <sch:title>The value [of id] must be unique among all 'group' id attribute values within the enclosing 'file' element</sch:title>
        <sch:rule context="xlf:group">
            <sch:let name="id" value="current()/@id"/>
            <sch:let name="fragid" value="concat('f=',ancestor::xlf:file/@id,'/g=',$id)"/>
            <sch:report diagnostics="fragid-report"  see="&this-loc;/&name;-&stage;.html#id"
                test="count(ancestor::xlf:file//xlf:group[@id=$id])>1">
                id duplication found. The value '<sch:value-of select="$id"/>' is used more than once for 'group' elements 
                within the enclosing 'file'.
            </sch:report>
        </sch:rule>
    </sch:pattern>
    <sch:pattern id="K3">
        <sch:title>The value [of id] must be unique among all 'unit' id attribute values within the enclosing 'file' element</sch:title>
        <sch:rule context="xlf:unit">
            <sch:let name="id" value="@id"/>
            <sch:let name="fragid" value="concat('f=', ancestor::xlf:file/@id,'/u=',$id)"/>
            <sch:report diagnostics="fragid-report" see="&this-loc;/&name;-&stage;.html#id"
                test="count(ancestor::xlf:file//xlf:unit[@id=$id])>1">
                id duplication found. The value '<sch:value-of select="$id"/>' is used more than once for 'unit' elements 
                within the enclosing 'file'.
            </sch:report>
        </sch:rule>
    </sch:pattern>
    <!-- K4F (file) and K4GU (group/unit) are distinguished for accurate fragID report -->
    <sch:pattern id="K4F">
        <sch:title>The value [of id] must be unique among all 'note' id attribute values within the enclosing 'notes' element</sch:title>
        <sch:rule context="xlf:note[@id][not(ancestor::xlf:unit)][not(ancestor::xlf:group)]">
            <sch:let name="id" value="@id"/>
            <sch:let name="fragid" value="concat('f=',ancestor::xlf:file/@id,'/n=',$id) "/>
            <sch:report diagnostics="fragid-report" see="&this-loc;/&name;-&stage;.htmll#id"
                test="following-sibling::xlf:note[@id=$id]">
                id duplication found. The value '<sch:value-of select="$id"/>' is used more than once for 'note' elements 
                within the enclosing 'notes'.
            </sch:report>
        </sch:rule>
    </sch:pattern>
    <sch:pattern id="K4GU">
        <sch:title>The value [of id] must be unique among all 'note' id attribute values within the enclosing 'notes' element</sch:title>
        <sch:rule context="xlf:note[@id][ancestor::xlf:unit | ancestor::xlf:group]">
            <sch:let name="id" value="current()/@id"/>
            <sch:let name="fragid" value="concat('f=',ancestor::xlf:file/@id,concat('/',substring(local-name(../..),1,1),'=',../../@id),'/n=',$id)"/>
            <sch:report diagnostics="fragid-report" see="&this-loc;/&name;-&stage;.html#id"
                test="following-sibling::xlf:note[@id=$id]">
                id duplication found. The value '<sch:value-of select="$id"/>' is used more than once for 'note' elements 
                within the enclosing 'notes'.
            </sch:report>
        </sch:rule>
    </sch:pattern>
    <!-- K5C (Core), K5MTC (MTC Module) and K5CTR (CTR Module) are distinguished for accurate fragID report and observing independency of the modules -->
    <sch:pattern id="K5C">
        <sch:title>The value [of id] must be unique among all 'data' id attribute values within the enclosing 'originalData' element</sch:title>
        <sch:rule context="xlf:data[local-name(../..)='unit']">
            <sch:let name="id" value="current()/@id"/>
            <sch:let name="fragid" value="concat('f=',ancestor::xlf:file/@id,'/u=', ancestor::xlf:unit/@id,'/d=',$id)"/>
            <sch:report diagnostics="fragid-report" see="&this-loc;/&name;-&stage;.html#id"
                test="following-sibling::xlf:data[@id=$id]">
                id duplication found. The value '<sch:value-of select="$id"/>' is used more than once for 'data' elements 
                within the enclosing 'originalData'.
            </sch:report>
        </sch:rule>
    </sch:pattern>
    <!-- K6S (source) and K6T (target) are distinguished for accurate fragID report -->
    <sch:pattern id="K6S">
        <sch:title>Except for the above exception [target duplication], the value [of id attribute] must be unique among all of the above 
            [segment, ignorable, mkr, sm,pc, sc, ec, ph] within the enclosing 'unit' element</sch:title>
        <sch:rule context="xlf:source[ancestor::xlf:segment| ancestor::xlf:ignorable]//xlf:*[@id]|
            xlf:segment[@id]| xlf:ignorable[@id]">
            <sch:let name="id" value="current()/@id"/>
            <sch:let name="fragid" value="concat('f=',ancestor::xlf:file/@id,'/u=',ancestor::xlf:unit/@id,'/',$id)"/>
            <sch:report diagnostics="fragid-report" see="&this-loc;/&name;-&stage;.html#id"
                test="count(ancestor::xlf:unit//xlf:*[@id=$id][ancestor-or-self::xlf:segment| ancestor-or-self::xlf:ignorable][not(ancestor::xlf:target)])>1">
                id duplication found. The value '<sch:value-of select="$id"/>' is used more then once among inline and/or 'segmen'/'ignorable' 
                elements within the enclosing 'unit'.
            </sch:report>
        </sch:rule>
    </sch:pattern>
    <sch:pattern id="K6T">
        <sch:title>The inline elements enclosed by a 'target' element must use the duplicate id values of their corresponding inline elements enclosed within the sibling 'source' element if and only if those corresponding elements exist</sch:title>
        <sch:rule context="xlf:target[ancestor::xlf:segment | ancestor::xlf:ignorable]//xlf:*[@id]">
            <sch:let name="id" value="@id"/>
            <sch:let name="file-id" value="ancestor::xlf:file/@id"/>
            <sch:let name="unit-id" value="ancestor::xlf:unit/@id"/>
            <sch:let name="fragid" value="concat('f=', $file-id,'/u=',$unit-id,'/t=',$id)"/>
            <sch:let name="counter" value="count(ancestor::xlf:unit//xlf:*[@id=$id]
                [ancestor-or-self::xlf:segment| ancestor-or-self::xlf:ignorable])"/>
            <!-- If the counter is 1, then the id is unique and therefore valid -->
            <!-- If the counter is 2 (duplication exsists), the duplication must be 
            in the sibling source, have the same name and contain same attributes-->
            <!-- If the counter is more than 2 or the duplication is not in the right 
            position, then an error will be reported -->
            <sch:assert diagnostics="fragid-report" see="&this-loc;/&name;-&stage;.html#id"
                test="($counter=1) or ($counter=2 and
                count(ancestor::xlf:segment/xlf:source//xlf:*[@id=$id]
                [local-name()= local-name(current())]| 
                ancestor::xlf:ignorable/xlf:source//xlf:*[@id=$id][local-name()= 
                local-name(current())])=1)">
                Invalid id used for element '<sch:name/>'. It must duplicate the 
                id of its corresponding element, enclosed within the 'source' 
                element or be unique in the scope of 'unit'.
            </sch:assert>
        </sch:rule>
    </sch:pattern>
    <sch:pattern id="K7">
        <sch:title>The value of the order attribute must be unique within the enclosing 'unit' element</sch:title>
        <sch:rule context="xlf:target[@order][ancestor::xlf:segment | ancestor::xlf:ignorable]">
            <sch:let name="order" value="@order"/>
            <sch:let name="fragid" value="concat('f=', ancestor::xlf:file/@id, '/u=', ancestor::xlf:unit/@id)"/>
            <sch:report diagnostics="fragid-report"  see="&this-loc;/&name;-&stage;.html#order"
                test="count(ancestor::xlf:unit//xlf:target[ancestor::xlf:segment|ancestor::xlf:ignorable][@order=$order])>1">
                The value '<sch:value-of select="$order"/>' is used more than once for 'order' attributes of 'target' elements 
                within the enclosing 'unit'.
            </sch:report>
        </sch:rule>
    </sch:pattern>
    <!-- End of Rules for primary keys -->
    
    <!-- Remaining Rules -->
    <sch:pattern id="F1">
        <sch:title>The 'trgLang' attribute is required if and only if the XLIFF Document contains 'target' elements 
            that are children of 'segment' or 'ignorable'</sch:title>
        <sch:rule context="xlf:target[parent::xlf:segment | parent::xlf:ignorable]">
            <sch:let name="fragid" value="concat('f=', ancestor::xlf:file/@id, '/u=', ancestor::xlf:unit/@id)"/>
            <sch:assert diagnostics="fragid-report" see="&this-loc;/&name;-&stage;.html#xliff" 
                test="/xlf:xliff/@trgLang">
                XLIFF document contains 'target' element(s), but the 'trgLang' attribute of 'xliff' is missing.
            </sch:assert>
        </sch:rule>
    </sch:pattern>
    <sch:pattern id="F2">
        <sch:title>The attribute 'href' is required if and only if the 'skeleton' element is empty</sch:title>
        <sch:rule context="xlf:skeleton">
            <sch:let name="fragid" value="concat('f=',ancestor::xlf:file/@id)"/>
            <sch:report diagnostics="fragid-report" see="&this-loc;/&name;-&stage;.html#skeleton"
                test="not(@href) and not(child::node())">
                'skeleton' element must not be empty when the 'href' attribute is missing.
            </sch:report>
            <sch:report diagnostics="fragid-report" see="&this-loc;/&name;-&stage;.html#skeleton"
                test="@href and  child::node()">
                'skeleton' element must be empty when containing 'href' attribute.
            </sch:report>
        </sch:rule>
    </sch:pattern>
    <sch:pattern id="F3">
        <sch:title>A 'unit' must contain at least one 'segment' element</sch:title>
        <sch:rule context="xlf:unit">
            <sch:let name="fragid" value="concat('f=',ancestor::xlf:file/@id,'/u=', current()/@id)"/>
            <sch:report  diagnostics="fragid-report" see="&this-loc;/&name;-&stage;.html#unit"
                test="not(child::xlf:segment)">
                Incomplete 'unit'; it must have at least one 'segment' child.
            </sch:report>
        </sch:rule>
    </sch:pattern>
    <sch:pattern id="F4">
        <sch:title>When a 'target' element is a child of 'segment' or 'ignorable', the explicit or inherited value
            of the optional xml:lang must be equal to the value of the trgLang attribute of the enclosing 'xliff' 
            element</sch:title>
        <sch:p>This rule does not rais an error if 'xml:lang' is a subcategory of 'trgLang', but not vice versa.
            i.e. 'trgLang="en"/xml:lang="en-ie"' is a valid pair.</sch:p>
        <sch:rule context="xlf:target[@xml:lang][parent::xlf:segment | parent::xlf:ignorable]">
            <sch:let name="trgLang" value="/xlf:xliff/@trgLang"/>
            <sch:let name="fragid" value="concat('f=',ancestor::xlf:file/@id,'/u=', ancestor::xlf:unit/@id)"/>
            <sch:report  diagnostics="fragid-report" see="&this-loc;/&name;-&stage;.html#target"
                test="not(lang($trgLang))">
                'xml:lang' attribute of the 'target' element and 'trgLang' attribute of the 'xliff' are not matching.
            </sch:report>
        </sch:rule>
    </sch:pattern>
    <sch:pattern id="F5S">
        <sch:title>The attribute isolated must be set to yes if and only if the 'ec' element corresponding to this
            start marker is not in the same 'unit', and set to no otherwise</sch:title>
        <sch:p>This rule selects those 'sc' with 'isolated' attribute set to 'yes' and appear in 'source'. 
            An error will be raised if there exists any 'ec' in the same 'unit' and in a 'source', corresponding to this 'sc' by 'startRef'</sch:p>
        <sch:rule context="xlf:sc[ancestor::xlf:source][@isolated='yes'][ancestor::xlf:segment | ancestor::xlf:ignorable]">
            <sch:let name="id" value="current()/@id"/>
            <sch:let name="file-id" value="ancestor::xlf:file/@id"/>
            <sch:let name="unit-id" value="ancestor::xlf:unit/@id"/>
            <sch:let name="fragid" value="concat('f=', $file-id, '/u=', $unit-id, '/', $id)"/>
            <sch:report diagnostics="fragid-report" see="&this-loc;/&name;-&stage;.html#isolated"
                test="ancestor::xlf:unit//xlf:ec[@startRef=$id][ancestor::xlf:source][ancestor::xlf:segment | ancestor::xlf:ignorable]">
                'isolated' attribute is set to 'yes', but 'ec' element(s) referencing this start code found within the same unit.
            </sch:report>
        </sch:rule>
    </sch:pattern>
    <sch:pattern id="F5T">
        <sch:title>The attribute isolated must be set to yes if and only if the 'ec' element corresponding to this
            start marker is not in the same 'unit', and set to no otherwise</sch:title>
        <sch:p>This rule selects those 'sc' with 'isolated' attribute set to 'yes' and appear in 'target'. 
            An error will be raised if there exists any 'ec' in the same 'unit' and in a 'target', corresponding to this 'sc' by 'startRef'</sch:p>
        <sch:rule context="xlf:sc[ancestor::xlf:target][@isolated='yes'][ancestor::xlf:segment | ancestor::xlf:ignorable]">
            <sch:let name="id" value="current()/@id"/>
            <sch:let name="file-id" value="ancestor::xlf:file/@id"/>
            <sch:let name="unit-id" value="ancestor::xlf:unit/@id"/>
            <sch:let name="fragid" value="concat('f=', $file-id, '/u=', $unit-id, '/t=', $id)"/>
            <sch:report diagnostics="fragid-report" see="&this-loc;/&name;-&stage;.html#isolated"
                test="ancestor::xlf:unit//xlf:ec[@startRef=$id][ancestor::xlf:target][ancestor::xlf:segment | ancestor::xlf:ignorable]">
                'isolated' attribute is set to 'yes', but 'ec' element(s) referencing this start code found within the same unit.
            </sch:report>
        </sch:rule>
    </sch:pattern>
    <sch:pattern id="F5.1S">
        <sch:title>The attribute isolated must be set to yes if and only if the 'ec' element corresponding to this
            start marker is not in the same 'unit', and set to no otherwise</sch:title>
        <sch:p>This rule selects those 'sc' with 'isolated' attribute set to 'no' (or missing) and appear in 'source'. 
            An error will be raised if
            a) there is not one and only one 'ec' element in the same 'unit', in a 'source', which appears after the 'sc' 
            and corresponds to this 'sc' by 'startRef';
            b) the values of 'canCopy', 'canDelete', 'canReorder' or 'canOverlap' attributes are not matching with the 
            corresponding ec;
            c) the value of 'canReorder' is set to 'firstNo', but is not 'no' in the corresponding 'ec'</sch:p>
        <sch:rule context="xlf:sc[ancestor::xlf:source][ancestor::xlf:segment | ancestor::xlf:ignorable][@isolated='no'] | 
            xlf:sc[ancestor::xlf:source][ancestor::xlf:segment | ancestor::xlf:ignorable][not(@isolated)]">
            <sch:let name="id" value="current()/@id"/>
            <sch:let name="file-id" value="ancestor::xlf:file/@id"/>
            <sch:let name="unit-id" value="ancestor::xlf:unit/@id"/>
            <sch:let name="fragid" value="concat('f=', $file-id, '/u=', $unit-id, '/', $id)"/>
            <sch:assert diagnostics="fragid-report" see="&this-loc;/&name;-&stage;.html#isolated"
                test="count(following::xlf:ec[@startRef=$id][ancestor::xlf:source]
                [ancestor::xlf:unit[@id=$unit-id]][ancestor::xlf:file[@id=$file-id]])=1">
                'isolated' attribute is set to 'not', but the corresponding 'ec' element within the same 'unit' was not found. 
                The end code must appear after the start code.
            </sch:assert>
            <sch:report diagnostics="fragid-report" see="&this-loc;/&name;-&stage;.html#sc"
                test="@canCopy='no' and not(following::xlf:ec[@startRef=$id][ancestor::xlf:source]
                [ancestor::xlf:unit/@id=$unit-id][ancestor::xlf:file/@id=$file-id][@canCopy='no'])">
                'canCopy' attribute is not matching with the 'canCopy' attribute of the corresponding 'ec' element within the same 'unit'.
            </sch:report>
            <sch:report diagnostics="fragid-report" see="&this-loc;/&name;-&stage;.html#sc"
                test="(@canCopy='yes' or not(@canCopy)) and not(following::xlf:ec[@startRef=$id][ancestor::xlf:source]
                [ancestor::xlf:unit/@id=$unit-id][ancestor::xlf:file/@id=$file-id][@canCopy='yes']) and 
                not(following::xlf:ec[@startRef=$id][ancestor::xlf:source][ancestor::xlf:unit/@id=$unit-id]
                [ancestor::xlf:file/@id=$file-id][not(@canCopy)])">
                'canCopy' attribute is not matching with the 'canCopy' attribute of the corresponding 'ec' element within the same 'unit'.
            </sch:report>
            <sch:report diagnostics="fragid-report" see="&this-loc;/&name;-&stage;.html#sc"
                test="@canDelete='no' and not(following::xlf:ec[@startRef=$id][ancestor::xlf:source]
                [ancestor::xlf:unit/@id=$unit-id][ancestor::xlf:file/@id=$file-id][@canDelete='no'])">
                'canDelete' attribute is not matching with the 'canDelete' attribute of the corresponding 'ec' element within the same 'unit'.
            </sch:report>
            <sch:report diagnostics="fragid-report" see="&this-loc;/&name;-&stage;.html#sc"
                test="(@canDelete='yes' or not(@canDelete)) and not(following::xlf:ec[@startRef=$id]
                [ancestor::xlf:source][ancestor::xlf:unit/@id=$unit-id][ancestor::xlf:file/@id=$file-id][@canDelete='yes']) and 
                not(following::xlf:ec[@startRef=$id][ancestor::xlf:source][ancestor::xlf:unit/@id=$unit-id]
                [ancestor::xlf:file/@id=$file-id][not(@canDelete)])">
                'canDelete' attribute is not matching with the 'canDelete' attribute of the corresponding 'ec' element within the same 'unit'.
            </sch:report>
            <sch:report diagnostics="fragid-report" see="&this-loc;/&name;-&stage;.html#sc"
                test="@canReorder='no' and not(following::xlf:ec[@startRef=$id][ancestor::xlf:source]
                [ancestor::xlf:unit/@id=$unit-id][ancestor::xlf:file/@id=$file-id][@canReorder='no'])">
                'canReorder' attribute is not matching with the 'canReorder' attribute of the corresponding 'ec' element within the same 'unit'.
            </sch:report>
            <sch:report diagnostics="fragid-report" see="&this-loc;/&name;-&stage;.html#sc"
                test="(@canReorder='yes' or not(@canReorder)) and not(following::xlf:ec[@startRef=$id]
                [ancestor::xlf:source][ancestor::xlf:unit/@id=$unit-id][ancestor::xlf:file/@id=$file-id][@canReorder='yes']) and 
                not(following::xlf:ec[@startRef=$id][ancestor::xlf:source][ancestor::xlf:unit/@id=$unit-id]
                [ancestor::xlf:file/@id=$file-id][not(@canReorder)])">
                'canReorder' attribute is not matching with the 'canReorder' attribute of the corresponding 'ec' element within the same 'unit'.
            </sch:report>
            <sch:report diagnostics="fragid-report" see="&this-loc;/&name;-&stage;.html#sc"
                test="@canOverlap='no' and not(following::xlf:ec[@startRef=$id][ancestor::xlf:source]
                [ancestor::xlf:unit/@id=$unit-id][ancestor::xlf:file/@id=$file-id][@canOverlap='no'])">
                'canOverlap' attribute is not matching with the 'canOverlap' attribute of the corresponding 'ec' element within the same 'unit'.
            </sch:report>
            <sch:report diagnostics="fragid-report" see="&this-loc;/&name;-&stage;.html#sc"
                test="(@canOverlap='yes' or not(@canOverlap)) and not(following::xlf:ec[@startRef=$id]
                [ancestor::xlf:source][ancestor::xlf:unit/@id=$unit-id][ancestor::xlf:file/@id=$file-id][@canOverlap='yes']) and 
                not(following::xlf:ec[@startRef=$id][ancestor::xlf:source][ancestor::xlf:unit/@id=$unit-id]
                [ancestor::xlf:file/@id=$file-id][not(@canOverlap)])">
                'canOverlap' attribute is not matching with the 'canOverlap' attribute of the corresponding 'ec' element within the same 'unit'.
            </sch:report>
            <sch:report diagnostics="fragid-report" see="&this-loc;/&name;-&stage;.html#ec"
                test="@canReorder='firstNo' and not(following::xlf:ec[@startRef=$id][ancestor::xlf:source][ancestor::xlf:unit/@id=$unit-id]
                [ancestor::xlf:file/@id=$file-id][@canReorder='no'])">
                'canReorder' is set to 'firstNo', but the corresponding 'ec', within the same 'unit'
                has not set its 'canReorder' attribute to 'no'.
            </sch:report>
        </sch:rule>
    </sch:pattern>
    <sch:pattern id="F5.1T">
        <sch:title>The attribute isolated must be set to yes if and only if the 'ec' element corresponding to this
            start marker is not in the same 'unit', and set to no otherwise</sch:title>
        <sch:p>This rule selects those 'sc' with 'isolated' attribute set to 'no' (or missing) and appear in 'target'. 
            An error will be raised if
            a) there is not one and only one 'ec' element in the same 'unit', in a 'target', which appears after the 'sc' 
            and corresponds to this 'sc' by 'startRef';
            b) the values of 'canCopy', 'canDelete', 'canReorder' or 'canOverlap' attributes are not matching with the 
            corresponding ec;
            c) the value of 'canReorder' is set to 'firstNo', but is not 'no' in the corresponding 'ec'</sch:p>
        <sch:rule context="xlf:sc[ancestor::xlf:target][ancestor::xlf:segment | ancestor::xlf:ignorable][@isolated='no'] | 
            xlf:sc[ancestor::xlf:target][ancestor::xlf:segment | ancestor::xlf:ignorable][not(@isolated)]">
            <sch:let name="id" value="current()/@id"/>
            <sch:let name="file-id" value="ancestor::xlf:file/@id"/>
            <sch:let name="unit-id" value="ancestor::xlf:unit/@id"/>
            <sch:let name="fragid" value="concat('f=', $file-id, '/u=', $unit-id, '/t=', $id)"/>
            <sch:assert diagnostics="fragid-report" see="&this-loc;/&name;-&stage;.html#isolated"
                test="count(following::xlf:ec[@startRef=$id][ancestor::xlf:target]
                [ancestor::xlf:unit[@id=$unit-id]][ancestor::xlf:file[@id=$file-id]])=1">
                'isolated' attribute is set to 'not', but the corresponding 'ec' element within the same 'unit' was not found.
                The end code must appear after the start code.
            </sch:assert>
            <sch:report diagnostics="fragid-report" see="&this-loc;/&name;-&stage;.html#sc"
                test="@canCopy='no' and not(following::xlf:ec[@startRef=$id][ancestor::xlf:target]
                [ancestor::xlf:unit/@id=$unit-id][ancestor::xlf:file/@id=$file-id][@canCopy='no'])">
                'canCopy' attribute is not matching with the 'canCopy' attribute of the corresponding 'ec' element within the same 'unit'.
            </sch:report>
            <sch:report diagnostics="fragid-report" see="&this-loc;/&name;-&stage;.html#sc"
                test="(@canCopy='yes' or not(@canCopy)) and not(following::xlf:ec[@startRef=$id]
                [ancestor::xlf:target][ancestor::xlf:unit/@id=$unit-id][ancestor::xlf:file/@id=$file-id][@canCopy='yes']) and 
                not(following::xlf:ec[@startRef=$id][ancestor::xlf:target][ancestor::xlf:unit/@id=$unit-id]
                [ancestor::xlf:file/@id=$file-id][not(@canCopy)])">
                'canCopy' attribute is not matching with the 'canCopy' attribute of the corresponding 'ec' element within the same 'unit'.
            </sch:report>
            <sch:report diagnostics="fragid-report" see="&this-loc;/&name;-&stage;.html#sc"
                test="@canDelete='no' and not(following::xlf:ec[@startRef=$id][ancestor::xlf:target]
                [ancestor::xlf:unit/@id=$unit-id][ancestor::xlf:file/@id=$file-id][@canDelete='no'])">
                'canDelete' attribute is not matching with the 'canDelete' attribute of the corresponding 'ec' element within the same 'unit'.
            </sch:report>
            <sch:report diagnostics="fragid-report" see="&this-loc;/&name;-&stage;.html#sc"
                test="(@canDelete='yes' or not(@canDelete)) and not(following::xlf:ec[@startRef=$id]
                [ancestor::xlf:target][ancestor::xlf:unit/@id=$unit-id][ancestor::xlf:file/@id=$file-id][@canDelete='yes']) and 
                not(following::xlf:ec[@startRef=$id][ancestor::xlf:target][ancestor::xlf:unit/@id=$unit-id]
                [ancestor::xlf:file/@id=$file-id][not(@canDelete)])">
                The 'canDelete' attribute is not matching with the 'canDelete' attribute of the corresponding 'ec' element within the same 'unit'.
            </sch:report>
            <sch:report diagnostics="fragid-report" see="&this-loc;/&name;-&stage;.html#sc"
                test="@canReorder='no' and not(following::xlf:ec[@startRef=$id][ancestor::xlf:target]
                [ancestor::xlf:unit/@id=$unit-id][ancestor::xlf:file/@id=$file-id][@canReorder='no'])">
                'canReorder' attribute is not matching with the 'canReorder' attribute of the corresponding 'ec' element within the same 'unit'.
            </sch:report>
            <sch:report diagnostics="fragid-report" see="&this-loc;/&name;-&stage;.html#sc"
                test="(@canReorder='yes' or not(@canReorder)) and not(following::xlf:ec[@startRef=$id]
                [ancestor::xlf:target][ancestor::xlf:unit/@id=$unit-id][ancestor::xlf:file/@id=$file-id][@canReorder='yes']) and 
                not(following::xlf:ec[@startRef=$id][ancestor::xlf:target][ancestor::xlf:unit/@id=$unit-id]
                [ancestor::xlf:file/@id=$file-id][not(@canReorder)])">
                'canReorder' attribute is not matching with the 'canReorder' attribute of the corresponding 'ec' element within the same 'unit'.
            </sch:report>
            <sch:report diagnostics="fragid-report" see="&this-loc;/&name;-&stage;.html#sc"
                test="@canOverlap='no' and not(following::xlf:ec[@startRef=$id][ancestor::xlf:target]
                [ancestor::xlf:unit/@id=$unit-id][ancestor::xlf:file/@id=$file-id][@canOverlap='no'])">
                'canOverlap' attribute is not matching with the 'canOverlap' attribute of the corresponding 'ec' element within the same 'unit'.
            </sch:report>
            <sch:report diagnostics="fragid-report" see="&this-loc;/&name;-&stage;.html#sc"
                test="(@canOverlap='yes' or not(@canOverlap)) and not(following::xlf:ec[@startRef=$id]
                [ancestor::xlf:target][ancestor::xlf:unit/@id=$unit-id][ancestor::xlf:file/@id=$file-id][@canOverlap='yes']) and 
                not(following::xlf:ec[@startRef=$id][ancestor::xlf:target][ancestor::xlf:unit/@id=$unit-id]
                [ancestor::xlf:file/@id=$file-id][not(@canOverlap)])">
                The 'canOverlap' attribute is not matching with the 'canOverlap' attribute of the corresponding 'ec' element within the same 'unit'.
            </sch:report>
            <sch:report diagnostics="fragid-report" see="&this-loc;/&name;-&stage;.html#ec"
                test="@canReorder='firstNo' and not(following::xlf:ec[@startRef=$id][ancestor::xlf:target][ancestor::xlf:unit/@id=$unit-id]
                [ancestor::xlf:file/@id=$file-id][@canReorder='no'])">
                'canReorder' is set to 'firstNo', but the corresponding 'ec', within the same 'unit'
                has not set its 'canReorder' attribute to 'no'.
            </sch:report>
        </sch:rule>
    </sch:pattern>
    <sch:pattern id="F6S">
        <sch:title>The attribute isolated must be set to yes if and only if the 'sc' element corresponding to this
            end code [ec] is not in the same 'unit' and set to no otherwise</sch:title>
        <sch:p>This rule selects those 'ec' which are in 'source' and raises an error if
            a) 'id' and 'startRef' attributes are used illegally, based on the value of 'isolated': if 'yes',
            only 'id' must be used and if 'no' (or missing), only 'startRef' must be specified;
            b) 'isolated' is 'no' (or missing)) and the 'dir' attribute is used;
            c)'isolated' is set to 'no', but there is no 'sc' in the same 'unit', in a 'source' and which
            appears before this end code.
        </sch:p>
        <sch:rule context="xlf:ec[ancestor::xlf:source][ancestor::xlf:segment | ancestor::xlf:ignorable]">
            <sch:let name="file-id" value="ancestor::xlf:file/@id"/>
            <sch:let name="unit-id" value="ancestor::xlf:unit/@id"/>
            <sch:let name="fragid" value="concat('f=', $file-id, '/u=', $unit-id)"/>
            <sch:assert diagnostics="fragid-report" see="&this-loc;/&name;-&stage;.html#ec"
                test="(@isolated='yes' and @id and not(@startRef)) or 
                ((@isolated='no' or not(@isolated)) and @startRef and not(@id))">
                Illegal use of 'id' and 'startRef' atribute. If 'isolated' is set to 'yes', only 'id' must be 
                used and if set to 'no' (or missing), only 'startRef' must be specified
            </sch:assert>
            <sch:report diagnostics="fragid-report" see="&this-loc;/&name;-&stage;.html#ec"
                test="(@isolated='no' or not(@isolated)) and @dir">
                The 'dir' attribute may be used only when the attribute 'isolated' is set to 'yes'
            </sch:report>
            <sch:report diagnostics="fragid-report" see="&this-loc;/&name;-&stage;.html#ec"
                test="(@isolated='no' or not(@isolated)) and count(preceding::xlf:sc[@id=current()/@startRef][ancestor::xlf:source][ancestor::xlf:segment | ancestor::xlf:ignorable]
                [ancestor::xlf:file/@id=$file-id][ancestor::xlf:unit/@id=$unit-id][not(@isolated='yes')])!=1">
                'isolated' attribute is set to 'not', but the corresponding 'sc' element within the same 'unit' was not found. 
                The start code must appear befor the end code
            </sch:report>
        </sch:rule>
    </sch:pattern>
    <sch:pattern id="F6T">
        <sch:title>The attribute isolated must be set to yes if and only if the 'sc' element corresponding to this
            end code [ec] is not in the same 'unit' and set to no otherwise</sch:title>
        <sch:p>This rule selects those 'ec' which are in 'target' and raises an error if
            a) 'id' and 'startRef' attributes are used illegally, based on the value of 'isolated': if 'yes',
            only 'id' must be used and if 'no' (or missing), only 'startRef' must be specified;
            b)'isolated' is set to 'no' (or missing), but there is no 'sc' in the same 'unit', in a 'target' and which
            appears before this end code.
            c) 'isolate' is set to 'no' (or missing) and 'dir' attribute is used. 
        </sch:p>
        <sch:rule context="xlf:ec[ancestor::xlf:target][ancestor::xlf:segment | ancestor::xlf:ignorable]">
            <sch:let name="file-id" value="ancestor::xlf:file/@id"/>
            <sch:let name="unit-id" value="ancestor::xlf:unit/@id"/>
            <sch:let name="fragid" value="concat('f=', $file-id, '/u=', $unit-id)"/>
            <sch:assert diagnostics="fragid-report" see="&this-loc;/&name;-&stage;.html#ec"
                test="(@isolated='yes' and @id and not(@startRef)) or 
                ((@isolated='no' or not(@isolated)) and @startRef and not(@id))">
                Illegal use of 'id' and 'startRef' atribute. If 'isolated' is set to 'yes', only 'id' must be 
                used and if set to 'no' (or missing), only 'startRef' must be specified
            </sch:assert>
            <sch:report diagnostics="fragid-report" see="&this-loc;/&name;-&stage;.html#ec"
                test="(@isolated='no' or not(@isolated)) and @dir">
                The 'dir' attribute may be used only when the attribute 'isolated' is set to 'yes'
            </sch:report>
            <sch:report diagnostics="fragid-report" see="&this-loc;/&name;-&stage;.html#ec"
                test="(@isolated='no' or not(@isolated)) and count(preceding::xlf:sc[@id=current()/@startRef][ancestor::xlf:target][ancestor::xlf:segment | ancestor::xlf:ignorable]
                [ancestor::xlf:file/@id=$file-id][ancestor::xlf:unit/@id=$unit-id][not(@isolated='yes')])!=1">
                'isolated' attribute is set to 'not', but the corresponding 'sc' element within the same 'unit' was not found. 
                The start code must appear befor the end code.
            </sch:report>
        </sch:rule>
    </sch:pattern>
    <sch:pattern id="F7">
        <sch:title>If the attribute subState is used, the attribute state must be explicitly set</sch:title>
        <sch:rule context="xlf:segment[@subState]" >
            <sch:let name="fragid" value="concat('f=',ancestor::xlf:file/@id, '/u=',ancestor::xlf:unit/@id)"/>
            <sch:assert diagnostics="fragid-report" see="&this-loc;/&name;-&stage;.html#substate"
                test="@state" >
                'segment' element specifies 'subState' attribute, but missing the 'state' attribute.
            </sch:assert>
        </sch:rule>
    </sch:pattern>
    <sch:pattern id="F8">
        <sch:title>If the attribute 'subType' is used, the attribute 'type' must be specified as well</sch:title>
        <sch:p>This rule select inline elements, which specify 'subType'. It also checks the value of 'subType'.</sch:p>
        <sch:rule context="xlf:*[@subType][ancestor::xlf:segment | ancestor::xlf:ignorable]">
            <sch:let name="fragid" value="concat('f=',ancestor::xlf:file/@id,'/u=',ancestor::xlf:unit/@id)"/>
            <sch:assert diagnostics="fragid-report" see="&this-loc;/&name;-&stage;.html#subtype"
                test="@type">
                '<sch:name/>' element specifies 'subType' attribute, but missing the 'type' attribute.
            </sch:assert>
            <sch:assert diagnostics="fragid-report" see="&this-loc;/&name;-&stage;.html#subtype"
                test="((@subType='xlf:i' or @subType='xlf:u' or @subType='xlf:lb' or @subType='xlf:pb' or @subType='xlf:b')and( @type='fmt')) or
                (@subType='xlf:var' and @type='ui') or not(starts-with(@subType,'xlf:'))">
                'type' and 'subType' attributes don't match. Value of 'type' must either be set to a user-defined value, 
                or to 'ui' when 'subType=xlf:var', or to 'fmt' for following values of 'subType':'xlf:i','xlf:u','xlf:lb' and 'xlf:pb'.
            </sch:assert>
        </sch:rule>
    </sch:pattern>
    <sch:pattern id="F9">
        <sch:title>The copyOf attribute must be used when, and only when, the base code has no associated original data</sch:title>
        <sch:rule context="xlf:*[@copyOf][ancestor::xlf:segment | ancestor::xlf:ignorable]">
            <sch:let name="fragid" value="concat('f=',ancestor::xlf:file/@id,'/u=',ancestor::xlf:unit/@id)"/>
            <sch:report diagnostics="fragid-report" see="&this-loc;/&name;-&stage;.html#duplicatingexistingcode"
                test="local-name()='pc' and (@dataRefStart or @dataRefEnd)">
                'pc' cannot use 'copyOf' attribute while referring to the original data through 'dataRefStart/dataRefEnd' attributes.
            </sch:report>
            <sch:report diagnostics="fragid-report" see="&this-loc;/&name;-&stage;.html#duplicatingexistingcode"
                test="local-name()!='pc' and @dataRef">
                '<sch:name/>' cannot use 'copyOf' attribute while referring to the original data through 'dataRef' attribute.
            </sch:report>
        </sch:rule>
    </sch:pattern>
    <sch:pattern id="F10">
        <sch:title>When the attribute canReorder is set to no or firstNo, the attributes canCopy and canDelete must also be set to no</sch:title>
        <sch:rule context="xlf:*[@canReorder='no'][ancestor::xlf:segment | ancestor::xlf:ignorable] | xlf:*[@canReorder='firstNo'][ancestor::xlf:segment | ancestor::xlf:ignorable]">
            <sch:let name="fragid" value="concat('f=',ancestor::xlf:file/@id,'/u=',ancestor::xlf:unit/@id)"/>
            <sch:assert diagnostics="fragid-report" see="&this-loc;/&name;-&stage;.html#editinghints"
                test="@canDelete='no' and @canCopy='no'">
                '<sch:name/>' element has set its 'canReorder' attribute to '<sch:value-of select="@canReorder"/>', but 'canDelete' and 
                'canCopy' attributes are not set to 'no'.
            </sch:assert>
        </sch:rule>
    </sch:pattern>
    <sch:pattern id="F11S">
        <sch:title>Translate Annotation</sch:title>
        <sch:rule context="xlf:sm[@type='generic'][ancestor::xlf:source][ancestor::xlf:segment | ancestor::xlf:ignorable] | xlf:sm[not(@type)][ancestor::xlf:source][ancestor::xlf:segment | ancestor::xlf:ignorable] |
            xlf:mrk[@type='generic'][ancestor::xlf:source][ancestor::xlf:segment | ancestor::xlf:ignorable] | xlf:mrk[not(@type)][ancestor::xlf:source][ancestor::xlf:segment | ancestor::xlf:ignorable]">
            <sch:let name="fragid" value="concat('f=',ancestor::xlf:file/@id,'/u=',ancestor::xlf:unit/@id,'/',current()/@id)"/>
            <sch:assert diagnostics="fragid-report" see="&this-loc;/&name;-&stage;.html#translateAnnotation"
                test="@translate">
                '<sch:name/>' element is being used for translate annotaition, but missing the 'translate' attribute.
            </sch:assert>
        </sch:rule>
    </sch:pattern>
    <sch:pattern id="F11T">
        <sch:title>Translate Annotation</sch:title>
        <sch:rule context="xlf:sm[@type='generic'][ancestor::xlf:target][ancestor::xlf:segment | ancestor::xlf:ignorable]|xlf:sm[not(@type)][ancestor::xlf:target][ancestor::xlf:segment | ancestor::xlf:ignorable]|
            xlf:mrk[@type='generic'][ancestor::xlf:target][ancestor::xlf:segment | ancestor::xlf:ignorable] | xlf:mrk[not(@type)][ancestor::xlf:target][ancestor::xlf:segment | ancestor::xlf:ignorable]">
            <sch:let name="fragid" value="concat('f=',ancestor::xlf:file/@id,'/u=',ancestor::xlf:unit/@id,'/t=',current()/@id)"/>
            <sch:assert diagnostics="fragid-report" see="&this-loc;/&name;-&stage;.html#translateAnnotation"
                test="@translate">
                '<sch:name/>' element is being used for translate annotaition, but missing the 'translate' attribute.
            </sch:assert>
        </sch:rule>
    </sch:pattern>
    <sch:pattern id="F12S">
        <sch:title>Comment Annotation</sch:title>
        <sch:rule context="xlf:sm[@type='comment'][ancestor::xlf:source][ancestor::xlf:segment | ancestor::xlf:ignorable]|xlf:mrk[@type='comment'][ancestor::xlf:source][ancestor::xlf:segment | ancestor::xlf:ignorable]">
            <sch:let name="fragid" value="concat('f', ancestor::xlf:file/@id,'/u=',ancestor::xlf:unit/@id,'/',current()/@id)"/>
            <sch:assert diagnostics="fragid-report" see="&this-loc;/&name;-&stage;.html#commentAnnotation"
                test="(@value and not(@ref))or(@ref and not(@value))" >
                '<sch:name/>' element must contain one and only one of 'value' and 'ref' attributes when used for comment annotation.
            </sch:assert>
        </sch:rule>
    </sch:pattern>
    <sch:pattern id="F12T">
        <sch:title>Comment Annotation</sch:title>
        <sch:rule context="xlf:sm[@type='comment'][ancestor::xlf:target][ancestor::xlf:segment | ancestor::xlf:ignorable]|xlf:mrk[@type='comment'][ancestor::xlf:target][ancestor::xlf:segment | ancestor::xlf:ignorable]">
            <sch:let name="fragid" value="concat('f=',ancestor::xlf:file/@id,'/u=',ancestor::xlf:unit/@id,'/t=',current()/@id)"/>
            <sch:assert diagnostics="fragid-report" see="&this-loc;/&name;-&stage;.html#commentAnnotation"
                test="(@value and not(@ref))or(@ref and not(@value))">
                '<sch:name/>' element cannot contain both 'value' and 'ref' attributes simultaneously when used for comment annotation.
            </sch:assert>
        </sch:rule>
    </sch:pattern>
    <sch:pattern id="F13S">
        <sch:title>ref attribute in Comment Annotation</sch:title>
        <sch:rule context="xlf:sm[@type='comment'][@ref][ancestor::xlf:source][ancestor::xlf:segment | ancestor::xlf:ignorable] |
            xlf:mrk[@type='comment'][@ref][ancestor::xlf:source][ancestor::xlf:segment | ancestor::xlf:ignorable]">
            <sch:let name="ref" value="@ref"/>
            <sch:let name="fragid" value="concat('f=',ancestor::xlf:file/@id,'/u=',ancestor::xlf:unit/@id,'/',current()/@id)"/>
            <sch:assert diagnostics="fragid-report" see="&this-loc;/&name;-&stage;.html#commentAnnotation"
                test="(not(contains($ref,'#')) and count(ancestor::xlf:unit//xlf:note[@id=$ref])=1) or
                (contains($ref,'#') and not(contains($ref,'=')) and count(ancestor::xlf:unit//xlf:note[@id=substring-after($ref,'#')])=1) or
                (contains($ref,'#') and (contains($ref,'=')) and count(ancestor::xlf:unit//xlf:note[@id=substring-after($ref,'=')])=1)">
                'ref' attribute of '<sch:name/>' must point to a 'note' element within the same unit (allowed referencing patterns: ref="note-id", ref="#note-id" and ref="#n=note-id").
            </sch:assert>
        </sch:rule>
    </sch:pattern>
    <sch:pattern id="F13T">
        <sch:title>ref attribute in Comment Annotation</sch:title>
        <sch:rule context="xlf:sm[@type='comment'][@ref][ancestor::xlf:target][ancestor::xlf:segment | ancestor::xlf:ignorable] | 
            xlf:mrk[@type='comment'][@ref][ancestor::xlf:target][ancestor::xlf:segment | ancestor::xlf:ignorable]">
            <sch:let name="ref" value="@ref"/>
            <sch:let name="fragid" value="concat('f=',ancestor::xlf:file/@id,'/u=',ancestor::xlf:unit/@id,'/t=',current()/@id)"/>
            <sch:assert diagnostics="fragid-report" see="&this-loc;/&name;-&stage;.html#commentAnnotation"
                test="(not(contains($ref,'#')) and count(ancestor::xlf:unit//xlf:note[@id=$ref])=1) or
                (contains($ref,'#') and not(contains($ref,'=')) and count(ancestor::xlf:unit//xlf:note[@id=substring-after($ref,'#')])=1) or
                (contains($ref,'#') and (contains($ref,'=')) and count(ancestor::xlf:unit//xlf:note[@id=substring-after($ref,'=')])=1)">
                'ref' attribute of '<sch:name/>' must point to a 'note' element within the same unit (allowed referencing patterns: ref="note-id", ref="#note-id" and ref="#n=note-id").
            </sch:assert>
        </sch:rule>
    </sch:pattern>
    <sch:pattern id="F14">
        <sch:title>The 'copyOf' attribute must point to a code within the same 'unit'</sch:title>
        <sch:rule context="xlf:*[@copyOf][ancestor::xlf:segment | ancestor::xlf:ignorable]">
            <sch:let name="copyOf" value="@copyOf"/>
            <sch:let name="fragid" value="concat(ancestor::xlf:file/@id,'/u=',ancestor::xlf:unit/@id)"/>
            <sch:assert diagnostics="fragid-report" see="&this-loc;/&name;-&stage;.html#duplicatingexistingcode"
                test="ancestor::xlf:unit//xlf:*[ancestor::xlf:segment | ancestor::xlf:ignorable][@id=$copyOf]">
                No inline element with 'id=<sch:value-of select="$copyOf"/>' found within the same 'unit' (allowed referencing pattern: copyOf="id").
            </sch:assert>
            <sch:assert diagnostics="fragid-report" see="&this-loc;/&name;-&stage;.html#duplicatingexistingcode"
                test="ancestor::xlf:unit//xlf:*[ancestor::xlf:segment | ancestor::xlf:ignorable][@id=$copyOf][not(@dataRef)][not(@dataRefStart)][not(@dataRefEnd)]">
                If an inline code has associated original data (referes to a 'data' element), it cannot be replicated.
            </sch:assert>
            <sch:report diagnostics="fragid-report" see="&this-loc;/&name;-&stage;.html#duplicatingexistingcode"
                test="ancestor::xlf:unit//xlf:*[ancestor::xlf:segment | ancestor::xlf:ignorable][@id=$copyOf][@canCopy='no']">
                The 'canCopy' attribute of the inline element with id='<sch:value-of select="$copyOf"/>' is set to 'no' and therefore cannot be replicated.
            </sch:report>
        </sch:rule>
    </sch:pattern>
    <sch:pattern id="F15">
        <sch:title>dataRef attribute must point to a data element within the same unit</sch:title>
        <sch:rule context="xlf:*[@dataRef][ancestor::xlf:segment | ancestor::xlf:ignorable]">
            <sch:let name="fragid" value="concat('f=', ancestor::xlf:file/@id,'/u=',ancestor::xlf:unit/@id)"/>
            <sch:assert diagnostics="fragid-report" see="&this-loc;/&name;-&stage;.html#dataref"
                test="ancestor::xlf:unit/xlf:originalData/xlf:data[@id=current()/@dataRef]">
                'dataRef' attribute must point to a 'data' element within the same 'unit' (allowed referencing pattern: dataRef="data-id").
            </sch:assert>
        </sch:rule>
    </sch:pattern>
    <sch:pattern id="F16S">
        <sch:title>dataRefEnd attribute must point to a data element within the same unit</sch:title>
        <sch:rule context="xlf:pc[@dataRefEnd][ancestor::xlf:source][ancestor::xlf:segment | ancestor::xlf:ignorable]">
            <sch:let name="fragid" value="concat('f=', ancestor::xlf:file/@id,'/u=',ancestor::xlf:unit/@id,'/',current()/@id)"/>
            <sch:assert diagnostics="fragid-report" see="&this-loc;/&name;-&stage;.html#datarefend"
                test="ancestor::xlf:unit/xlf:originalData/xlf:data[@id=current()/@dataRefEnd]">
                'dataRefEnd' attribute must point to a 'data' element within the same 'unit' (allowed referencing pattern: dataRefEnd="data-id").
            </sch:assert>
            <sch:assert diagnostics="fragid-report" test="@dataRefStart">
                'dataRefEnd' and 'dataRefStart' must be used in pair.
            </sch:assert>
        </sch:rule>
    </sch:pattern>
    <sch:pattern id="F16T">
        <sch:title>dataRefEnd attribute must point to a data element within the same unit</sch:title>
        <sch:rule context="xlf:pc[@dataRefEnd][ancestor::xlf:target][ancestor::xlf:segment | ancestor::xlf:ignorable]">
            <sch:let name="fragid" value="concat('f=',ancestor::xlf:file/@id,'/u=',ancestor::xlf:unit/@id,'/t=',current()/@id)"/>
            <sch:assert diagnostics="fragid-report" see="&this-loc;/&name;-&stage;.html#datarefend"
                test="ancestor::xlf:unit/xlf:originalData/xlf:data[@id=current()/@dataRefEnd]">
                'dataRefEnd' attribute must point to a 'data' element within the same 'unit' (allowed referencing pattern: dataRefEnd="data-id").
            </sch:assert>
            <sch:assert diagnostics="fragid-report" test="@dataRefStart">
                'dataRefEnd' and 'dataRefStart' must be used in pair.
            </sch:assert>
        </sch:rule>
    </sch:pattern>
    <sch:pattern id="F17S">
        <sch:title>dataRefStart attribute must point to a data element within the same unit</sch:title>
        <sch:rule context="xlf:pc[@dataRefStart][ancestor::xlf:source][ancestor::xlf:segment | ancestor::xlf:ignorable]">
            <sch:let name="fragid" value="concat('f=',ancestor::xlf:file/@id,'/u=',ancestor::xlf:unit/@id,'/',current()/@id)"/>
            <sch:assert diagnostics="fragid-report" see="&this-loc;/&name;-&stage;.html#datarefstart"
                test="ancestor::xlf:unit/xlf:originalData/xlf:data[@id=current()/@dataRefStart]">
                'dataRefStart' attribute must point to a 'data' element within the same 'unit' (allowed referencing pattern: dataRefStart="data-id").
            </sch:assert>
            <sch:assert diagnostics="fragid-report" test="@dataRefEnd">
                'dataRefEnd' and 'dataRefStart' must be used in pair.
            </sch:assert>
        </sch:rule>
    </sch:pattern>
    <sch:pattern id="F17T">
        <sch:title>dataRefStart attribute must point to a data element within the same unit</sch:title>
        <sch:rule context="xlf:pc[@dataRefStart][ancestor::xlf:target][ancestor::xlf:segment | ancestor::xlf:ignorable]">
            <sch:let name="fragid" value="concat('f=',ancestor::xlf:file/@id,'/u=',ancestor::xlf:unit/@id,'/t=',current()/@id)"/>
            <sch:assert diagnostics="fragid-report" see="&this-loc;/&name;-&stage;.html#datarefstart"
                test="ancestor::xlf:unit/xlf:originalData/xlf:data[@id=current()/@dataRefStart]">
                'dataRefStart' attribute must point to a 'data' element within the same 'unit' (allowed referencing pattern: dataRefStart="data-id").
            </sch:assert>
            <sch:assert diagnostics="fragid-report" test="@dataRefEnd">
                'dataRefEnd' and 'dataRefStart' must be used in pair.
            </sch:assert>
        </sch:rule>
    </sch:pattern>
    <sch:pattern id="F18">
        <sch:title>Its value [order attribute] is an integer from 1 to N, where N is the sum of the numbers of the 'segment' 
            and 'ignorable' elements within the given enclosing 'unit' element</sch:title>
        <sch:rule context="xlf:unit">
            <sch:let name="maxOrder" value="count(xlf:segment|xlf:ignorable)"/>
            <sch:let name="fragid" value="concat('f=',ancestor::xlf:file/@id,'/u=',current()/@id)"/>
            <sch:report diagnostics="fragid-report" see="&this-loc;/&name;-&stage;.html#segorder"
                test="descendant::xlf:target[@order>$maxOrder][ancestor::xlf:segment|ancestor::xlf:ignorable]">
                Invalid value used for order attribute of 'target' element(s). It must be an integer from 1 to <sch:value-of select="$maxOrder"/>.
            </sch:report>
        </sch:rule>
    </sch:pattern>
    <sch:pattern id="F19">
        <sch:title>To be able to map order differences, the 'target' element has an optional order attribute that indicates its position in the sequence of segments (and inter-segments). Its value is an
            integer from 1 to N, where N is the sum of the numbers of the 'segment' and 'ignorable' elements within the given enclosing 'unit' element.
            When Writers set explicit order on 'target' elements, they have to check for conflicts with implicit order, as 'target' elements without explicit
            order correspond to their sibling 'source' elements</sch:title>
        <sch:rule context="xlf:target[@order][ancestor::xlf:segment | ancestor::xlf:ignorable]">
            <sch:let name="order" value="@order"/>
            <sch:let name="actual-pos" value="count(../preceding-sibling::xlf:segment| ../preceding-sibling::xlf:ignorable)+1"/>
            <sch:let name="fragid" value="concat('f=',ancestor::xlf:file/@id,'/u=',current()/@id)"/>
            <sch:assert diagnostics="fragid-report" see="&this-loc;/&name;-&stage;.html#segorder"
                test="ancestor::xlf:unit//xlf:target[@order=$actual-pos][ancestor::xlf:segment | ancestor::xlf:ignorable]"> 
                The corresponding 'target' element, 'order' attribute of which must be '<sch:value-of select="$actual-pos"/>', is missing within the enclosing 'unit'.
            </sch:assert>
        </sch:rule>
    </sch:pattern>
    <sch:pattern id="F20">
        <sch:title>Modifiers must not delete inline codes that have their attribute canDelete set to no</sch:title>
        <sch:rule context="xlf:*[@canDelete='no'][ancestor::xlf:segment][ancestor::xlf:source]">
            <sch:let name="id" value="current()/@id"/>
            <sch:let name="fragid" value="concat('f=', ancestor::xlf:file/@id,'/u=', ancestor::xlf:unit/@id,'/', $id)"/>
            <sch:report diagnostics="fragid-report" see="&this-loc;/&name;-&stage;.html#editinghints" role=""
                test="(ancestor::xlf:segment/@state='final') and 
                (not(ancestor::xlf:segment/xlf:target//xlf:*[@id=$id][local-name()=local-name(current())]))">
                When a 'segment' is at the final stage ('state' attribute set to 'final'), all inline elements with 'canDelete' attribute set to 'no'must have their corresponding elements in the sibling 'target'.
            </sch:report>
        </sch:rule>
    </sch:pattern>
    <sch:pattern id="F20W">
        <sch:title>Modifiers must not delete inline codes that have their attribute canDelete set to no</sch:title>
        <sch:rule context="xlf:*[@canDelete='no'][ancestor::xlf:segment][ancestor::xlf:source]" role="warn">
            <sch:let name="id" value="current()/@id"/>
            <sch:let name="fragid" value="concat('f=', ancestor::xlf:file/@id,'/u=', ancestor::xlf:unit/@id,'/', $id)"/>
            <sch:report diagnostics="fragid-report" see="&this-loc;/&name;-&stage;.html#editinghints" role="warning"
                test="(ancestor::xlf:segment[not(@state='final')]) and 
                (not(ancestor::xlf:segment/xlf:target//xlf:*[@id=$id][local-name()=local-name(current())]))">
                WARNING: 'canDelete' attribute is set to 'no', but the corresponding element is missing in the sibling target (not an error since 'state' attribute of the containing 'segment' is not 
                final').
            </sch:report>
        </sch:rule>
    </sch:pattern>
    <sch:pattern id="F21">
        <sch:title>subFlows must point to units within the same file</sch:title>
        <sch:rule context="xlf:*[@subFlows][ancestor::xlf:segment|ancestor::xlf:ignorable]">
            <sch:let name="fragid" value="concat('f=', ancestor::xlf:file/@id, '/u=', ancestor::xlf:unit/@id)"/>
            <sch:let name="sub-flows" value=" normalize-space(@subFlows)"/>
            <sch:assert diagnostics="fragid-report" see="&this-loc;/&name;-&stage;.html#subFlows" 
                test="count(tokenize($sub-flows,' '))=count(ancestor::xlf:file//xlf:unit[@id=tokenize($sub-flows,' ')])">
                The 'subFlows' attribute must contain a space-seperated list of 'unit' identifiers within the same 'file'.
            </sch:assert>
        </sch:rule>
    </sch:pattern>
    <sch:pattern id="F22">
        <sch:title>subFlowsEnd must point to units within the same file</sch:title>
        <sch:rule context="xlf:pc[@subFlowsEnd][ancestor::xlf:segment|ancestor::xlf:ignorable]">
            <sch:let name="fragid" value="concat('f=', ancestor::xlf:file/@id, '/u=', ancestor::xlf:unit/@id)"/>
            <sch:let name="sub-flows" value=" normalize-space(@subFlowsEnd)"/>
            <sch:assert diagnostics="fragid-report" see="&this-loc;/&name;-&stage;.html#subFlows" 
                test="count(tokenize($sub-flows,' '))=count(ancestor::xlf:file//xlf:unit[@id=tokenize($sub-flows,' ')])">
                The 'subFlowsEnd' attribute must contain a space-seperated list of 'unit' identifiers within the same 'file'.
            </sch:assert>
            <sch:assert test="@subFlowsStart">
                'subFlowsStart' and 'subFlowsEnd' must be used in pair.
            </sch:assert>
        </sch:rule>
    </sch:pattern>
    <sch:pattern id="F23">
        <sch:title>subFlowsStart must point to units within the same file</sch:title>
        <sch:rule context="xlf:pc[@subFlowsStart][ancestor::xlf:segment|ancestor::xlf:ignorable]">
            <sch:let name="fragid" value="concat('f=', ancestor::xlf:file/@id, '/u=', ancestor::xlf:unit/@id)"/>
            <sch:let name="sub-flows" value=" normalize-space(@subFlowsStart)"/>
            <sch:assert diagnostics="fragid-report" see="&this-loc;/&name;-&stage;.html#subFlows" 
                test="count(tokenize($sub-flows,' '))=count(ancestor::xlf:file//xlf:unit[@id=tokenize($sub-flows,' ')])">
                The 'subFlowsStart' attribute must contain a space-seperated list of 'unit' identifiers within the same 'file'.
            </sch:assert>
            <sch:assert test="@subFlowsEnd">
                'subFlowsStart' and 'subFlowsEnd' must be used in pair.
            </sch:assert>
        </sch:rule>
    </sch:pattern>
    <sch:pattern id="F24">
        <sch:title>Translation Candidate Annotation</sch:title>
        <sch:rule context="xlf:sm[@type='mtc:match'][ancestor::xlf:segment|ancestor::xlf:ignorable] | xlf:mrk[@type='mtc:match'][ancestor::xlf:segment|ancestor::xlf:ignorable]">
            <sch:report test="@ref">
                'ref' attribute is not allowed in Translation Candidate Annotation, when the 'type' attribute is set to 'mtc:match'.
            </sch:report>
        </sch:rule>
    </sch:pattern>
    <sch:pattern id="F25">
        <sch:rule context="xlf:cp">
            <sch:assert test="matches(@hex,'^([0-9a-fA-F][0-9a-fA-F][0-9a-fA-F][0-9a-fA-F]|[0-9a-fA-F][0-9a-fA-F][0-9a-fA-F][0-9a-fA-F][0-9a-fA-F]|[01][0-9a-fA-F][0-9a-fA-F][0-9a-fA-F][0-9a-fA-F][0-9a-fA-F])$')">
                The value of 'hex' attribute must be in  the hexadecimal inclusive range of 0000 to 10FFFF. 
            </sch:assert>
        </sch:rule>
    </sch:pattern>
    <sch:pattern id="F26">
        <sch:rule context="xlf:pc[@equivStart] | xlf:pc[@equivEnd]">
            <sch:assert test="@equivStart and @equivEnd">
                The pair of 'equivStart'/'equivEnd' attributes must be specified together.
            </sch:assert>
        </sch:rule>
    </sch:pattern>
    <sch:pattern id="F27">
        <sch:rule context="xlf:sm">
            <sch:let name="id" value="current()/@id"/>
            <sch:let name="file-id" value="ancestor::xlf:file/@id"/>
            <sch:let name="unit-id" value="ancestor::xlf:unit/@id"/>
            <sch:assert test="count(following::xlf:em[@startRef=$id][ancestor::xlf:unit/@id=$unit-id][ancestor::xlf:file/@id=$file-id])!=0">
                There must be an 'em' element corresponding to this start marker by the 'startRef' attribute (the end marker must appear in the same 'unit' and after the start code).
            </sch:assert>
        </sch:rule>
    </sch:pattern>
    <sch:pattern id="F28">
        <sch:rule context="xlf:em">
            <sch:let name="id" value="current()/@startRef"/>
            <sch:let name="file-id" value="ancestor::xlf:file/@id"/>
            <sch:let name="unit-id" value="ancestor::xlf:unit/@id"/>
            <sch:assert test="count(preceding::xlf:sm[@id=$id][ancestor::xlf:unit/@id=$unit-id][ancestor::xlf:file/@id=$file-id])!=0">
                There must be an 'sm' element corresponding to this end marker by the 'id' attribute (the start marker must appear in the same 'unit' and before the end code).
            </sch:assert>
        </sch:rule>
    </sch:pattern>
    <sch:pattern id="F29">
        <sch:rule context="xlf:segment[@state='translated' or @state='reviewed' or @state='final']">
            <sch:assert test="child::xlf:target">
                Incorrect usage of 'state' attribute; when it is set to 'translated', 'reviewed' or 'final', the 'segment' must contain 'target' child. 
            </sch:assert>
        </sch:rule>
    </sch:pattern>
    <sch:pattern id="F30">
        <sch:rule context="xlf:*[@canReorder='no']">
            <sch:let name="file-id" value="ancestor::xlf:file/@id"/>
            <sch:let name="unit-id" value="ancestor::xlf:unit/@id"/>
            <sch:report test="preceding::xlf:pc[position()=1][@canReorder='yes'] or preceding::xlf:pc[position()=1][not(@canReorder)] or 
                preceding::xlf:ph[position()=1][@canReorder='yes'] or preceding::xlf:ph[position()=1][not(@canReorder)] or 
                preceding::xlf:sc[position()=1][@canReorder='yes'] or preceding::xlf:sc[position()=1][not(@canReorder)] or 
                preceding::xlf:ec[position()=1][@canReorder='yes'] or preceding::xlf:ec[position()=1][not(@canReorder)]">
                Invalid non-reorderable sequence; inline element with 'canReorder' set to 'no' must not be preceded with inline element with 'canReorder' set to 'yes' or missing
            </sch:report>
            <sch:assert test="count(preceding::xlf:*[ancestor::xlf:file/@id=$file-id][ancestor::xlf:unit/@id=$unit-id][@canReorder='firstNo'])!=0">
                Invalid non-reorderable sequence; missing the start element with 'canReorder' set to 'firstNo' within the same 'unit.
            </sch:assert>
        </sch:rule>
    </sch:pattern>
    <!-- End of Schematron Rules -->
    <!-- Diagnostics used to provide fragment identification notations in accordance with sec. 3 of XLIFF Specification -->
    <sch:diagnostics>
        <sch:diagnostic id="fragid-report">#<sch:value-of select="$fragid"/></sch:diagnostic>
    </sch:diagnostics>
</sch:schema>