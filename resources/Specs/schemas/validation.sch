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
    <sch:title>Schematron rules for checking the constraints of the Validation module against XLIFF Version &version;</sch:title>
    <sch:ns prefix="val" uri="urn:oasis:names:tc:xliff:validation:2.0"/>
    <sch:ns prefix="xlf" uri="urn:oasis:names:tc:xliff:document:2.0"/>    
    <sch:pattern id="val1">
        <sch:rule context="val:rule" see="&this-loc;/&name;-&stage;.html#val_rule">
            <sch:let name="fragid" value="concat('f=',ancestor::xlf:file/@id)"/>
            <sch:report diagnostics="fragid-report" test="@isPresent and (@isNotPresent or @startsWith or @ endsWith)">
                Only one of isPresent, isNotPresent, startsWith or endsWith is allowed.
            </sch:report>
            <sch:report diagnostics="fragid-report" test="@isNotPresent and (@isPresent or @startsWith or @ endsWith)">
                Only one of isPresent, isNotPresent, startsWith or endsWith is allowed.
            </sch:report>
            <sch:report diagnostics="fragid-report" test="@startsWith and (@isPresent or @isNotPresent  or @ endsWith)">
                Only one of isPresent, isNotPresent, startsWith or endsWith is allowed.
            </sch:report>
            <sch:report diagnostics="fragid-report" test="@endsWith and (@isPresent or @isNotPresent  or @ startsWith)">
                Only one of isPresent, isNotPresent, startsWith or endsWith is allowed.
            </sch:report>
            <sch:report diagnostics="fragid-report" test="not(@isPresent) and not(@isNotPresent) and not(@startsWith) and not(@ endsWith) and not(@*[namespace-uri()!='urn:oasis:names:tc:xliff:validation:2.0'])">
                When native module attributes isPresent, isNotPresent, startsWith or endsWith are not used, a custom attribute must be set for 'val:rule' element.
            </sch:report>
        </sch:rule>
    </sch:pattern>
    <sch:pattern id="val2">
        <sch:rule context="val:rule[@occurs]">
            <sch:let name="fragid" value="concat('f=',ancestor::xlf:file/@id)"/>
            <sch:report diagnostics="fragid-report" see="&this-loc;/&name;-&stage;.html#val_occurs" test="not(@isPresent)">
                The occure attribute can only be used in pair with isPresent
            </sch:report>
        </sch:rule>
    </sch:pattern>
    <sch:pattern id="val3">
        <sch:rule context="val:rule[@existsInSource]">
            <sch:let name="fragid" value="concat('f=',ancestor::xlf:file/@id)"/>
            <sch:assert diagnostics="fragid-report" see="&this-loc;/&name;-&stage;.html#val_existsInSource"
                test="(@isPeresent and not(@startsWith) and not(@endsWith)) or 
                              (@startsWith and not(@isPeresent) and not(@endsWith)) or 
                              (@endsWith and not(@isPeresent) and not(@startsWith))">
                The 'existsInSourc'e attribute must be used with exactly one of 'isPresent', 'startsWith' or 'endsWith' attributes. 
            </sch:assert>
        </sch:rule> 
    </sch:pattern>
    <sch:pattern id="val4">
        <sch:let name="fragid" value="concat('f=',ancestor::xlf:file/@id)"/>
        <sch:rule context="val:rule[@disabled='yes']">
            <sch:report diagnostics="fragid-report" see="&this-loc;/&name;-&stage;.html#val_disabled"
                test="local-name(../..)='file'">
                The disabled attribute cannot be set to 'yes' when the Validation Module is at the 'file' level.
            </sch:report>
        </sch:rule>
    </sch:pattern>
    <!--<sch:pattern>
        <sch:rule context="val:rule[@startsWith][not(@disabled) or @disabled='no']">
            <sch:let name="start-pattern" value="@startsWith"/>
            <sch:let name="eligible-targets" value="count(../..//xlf:target[parent::xlf:segment][not(ancestor::xlf:group/val:validation/val:rule[@startsWith=$start-pattern][@disabled='yes'])][not(ancestor::xlf:unit/val:validation/val:rule[@startsWith=$start-pattern][@disabled='yes'])])"/>
            <sch:let name="valid-targets" value="count(../..//xlf:target[parent::xlf:segment][not(ancestor::xlf:group/val:validation/val:rule[@startsWith=$start-pattern][@disabled='yes'])][not(ancestor::xlf:unit/val:validation/val:rule[@startsWith=$start-pattern][@disabled='yes'])][starts-with(.,$start-pattern)])"/>
            <sch:let name="eligible-sources" value="count(../..//xlf:source[parent::xlf:segment][not(ancestor::xlf:group/val:validation/val:rule[@startsWith=$start-pattern][@disabled='yes'])][not(ancestor::xlf:unit/val:validation/val:rule[@startsWith=$start-pattern][@disabled='yes'])])"/>
            <sch:let name="valid-sources" value="count(../..//xlf:source[parent::xlf:segment][not(ancestor::xlf:group/val:validation/val:rule[@startsWith=$start-pattern][@disabled='yes'])][not(ancestor::xlf:unit/val:validation/val:rule[@startsWith=$start-pattern][@disabled='yes'])][starts-with(.,$start-pattern)])"/>
            <sch:assert test="((not(@existsInSource) or @existsInSource='no')) or
                (@existsInSource='yes' and $eligible-sources=$valid-sources) and $valid-targets=$eligible-targets">
                The startsWith pattern is violated.
            </sch:assert>
        </sch:rule>
    </sch:pattern>-->
   <!-- <sch:pattern>
        <sch:rule context="val:rule[@isPresent][local-name(../..)='file']">
            <sch:let name="isPresent" value="current()/@isPresent"/>
            <sch:let name="unit-override" value="count(../..//xlf:unit[descendant::val:rule[@isPresent=$isPresent][@disabled='yes']])"/>
            <sch:let name="group-override" value="count(../..//xlf:group[descendant::val:rule[@isPresent=$isPresent][@disabled='yes']])"/>
            <sch:let name="tester" value="../..//xlf:target"/>
            <sch:report test="true()">
                REPORTER <sch:value-of select="$override"/>
            </sch:report>
        </sch:rule>
    </sch:pattern>-->
    
    <sch:diagnostics>
        <sch:diagnostic id="spec-quote"></sch:diagnostic>
        <sch:diagnostic id="fragid-report">#<sch:value-of select="$fragid"/></sch:diagnostic>
    </sch:diagnostics>
</sch:schema>