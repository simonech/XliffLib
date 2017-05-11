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


<sch:schema   
    xmlns:sch="http://purl.oclc.org/dsdl/schematron"
    xmlns:xlf="urn:oasis:names:tc:xliff:document:2.0"
    xmlns:mtc="urn:oasis:names:tc:xliff:matches:2.0"
    xmlns:ctr="urn:oasis:names:tc:xliff:changetracking:2.1"
	xmlns:res="urn:oasis:names:tc:xliff:resourcedata:2.0"
	xmlns:its="http://www.w3.org/2005/11/its"
	xmlns:itsm="urn:oasis:names:tc:xliff:itsm:2.1"
    queryBinding='xslt2' 
    schemaVersion='ISO19757-3'>
    <sch:title>Schematron rules for checking the constraints of the ITS module against XLIFF Version &version;</sch:title>
    <sch:ns prefix="its" uri="http://www.w3.org/2005/11/its"/>
    <sch:ns prefix="xlf" uri="urn:oasis:names:tc:xliff:document:2.0"/>
    <sch:ns prefix="ctr" uri="urn:oasis:names:tc:xliff:changetracking:2.1"/>
    <sch:ns prefix="itsm" uri="urn:oasis:names:tc:xliff:itsm:2.1"/>
    
    <!-- ITS rules for mapping ITS data categories used in XLIFF 2.x. The rules are to be consumed by an ITS 2.0 conformant processor. The rules are ordered following the data category sub sections of the ITS module section in the XLIFF 2.1 specification.-->
    
    <its:rules version="2.0" queryLanguage="xpath">
        
        <!-- Rules for Allowed Characters are not needed, but covered by the W3C ITS namespace. -->
        
        <!-- Rules for Domain -->
        <its:domainRule selector="//xlf:mrk[@type='its:generic' and (@itsm:domains)]" domainPointer="@itsm:domains"/>
        
        <!-- Rules for Localization Quality Issue are not needed, but covered by the W3C ITS namespace. --> 
        
        <!-- Rules for Localization Quality Rating are not needed, but covered by the W3C ITS namespace. -->
        
        <!-- Rules for Text Analysis are not needed, but covered by the W3C ITS namespace. -->
        
        <!-- Rules for Localization Note. These rules cover the following cases:
            
        1) Note at file, no appliesTo attribute set: note relates to complete content of the file.
        2) Note at group, no appliesTo attribute set: Note relates to complete content of the group. 
        3) Note at unit, appliesTo=target: note relates to target elements in unit. 
        4) Note at file, appliesTo=target: note relates to target elements in file.
        5) Note at group, appliesTo=target: note relates to target elements in group.
        6) Note at unit, appliesTo=source: note relates to source elements in unit. 
        7) Note at file, appliesTo=source: note relates to source elements in file.
        8) Note at group, appliesTo=source: note relates to source elements in group.
        9) Annotation of the type="comment" with the Localization Note in the value attribute
                
        A priority attribute can appear on XLIFF <note>, it can be an integer 1-10. Value "1" means ITS alert and any other number ITS description.
        -->
        
        <!-- Start of notes of type "alert" -->
        
        <its:locNoteRule selector="//xlf:file//::xlf:source[ancestor::xlf:file[not(@appliesTo)]/xlf:notes/xlf:note[@priority=1]] | //xlf:file[not(@appliesTo)]//::xlf:target[ancestor::xlf:file[not(@appliesTo)]/xlf:notes/xlf:note[@priority=1]]" localizationNotePointer="ancestor::xlf:file[not(@appliesTo)]/xlf:notes/xlf:note" locNoteType="alert"/>
        
        <its:locNoteRule selector="//xlf:group//::xlf:source[ancestor::xlf:group[not(@appliesTo)]/xlf:notes/xlf:note[priority=1]] | //xlf:group[not(@appliesTo)]//::xlf:target[ancestor::xlf:group[not(@appliesTo)]/xlf:notes/xlf:note[@priority=1]]" localizationNotePointer="ancestor::xlf:group[not(@appliesTo)]/xlf:notes/xlf:note" locNoteType="alert"/>
        
        <its:locNoteRule selector="//xlf:unit[@appliesTo='target']//::xlf:target[ancestor::xlf:unit[@appliesTo='target']/xlf:notes/xlf:note[@priority=1]]" localizationNotePointer="ancestor::xlf:unit[@appliesTo='target']/xlf:notes/xlf:note" locNoteType="alert"/>
        
        <its:locNoteRule selector="//xlf:file[@appliesTo='target']//::xlf:target[ancestor::xlf:file[@appliesTo='target']/xlf:notes/xlf:note[@priority=1]]" localizationNotePointer="ancestor::xlf:file[@appliesTo='target']/xlf:notes/xlf:note" locNoteType="alert"/>
        
        <its:locNoteRule selector="//xlf:group[@appliesTo='target']//::xlf:target[ancestor::xlf:group[@appliesTo='target']/xlf:notes/xlf:note[@priority=1]]" localizationNotePointer="ancestor::xlf:group[@appliesTo='target']/xlf:notes/xlf:note" locNoteType="alert"/>
        
        <its:locNoteRule selector="//xlf:unit[@appliesTo='source']//::xlf:source[ancestor::xlf:unit[@appliesTo='source']/xlf:notes/xlf:note[@priority=1]]" localizationNotePointer="ancestor::xlf:unit[@appliesTo='source']/xlf:notes/xlf:note" locNoteType="alert"/>
        
        <its:locNoteRule selector="//xlf:file[@appliesTo='source']//::xlf:source[ancestor::xlf:file[@appliesTo='source']/xlf:notes/xlf:note[@priority=1]]" localizationNotePointer="ancestor::xlf:file[@appliesTo='source']/xlf:notes/xlf:note" locNoteType="alert"/>
        
        <its:locNoteRule selector="//xlf:group[@appliesTo='source']//::xlf:source[ancestor::xlf:group[@appliesTo='source']/xlf:notes/xlf:note[@priorty=1]]" localizationNotePointer="ancestor::xlf:group[@appliesTo='source']/xlf:notes/xlf:note" locNoteType="alert"/>        
        
        <its:locNoteRule selector="//xlf:*[@annotation='comment' and @value]" localizationNotePointer="@value" locNoteType="description"/>
        
        <!-- Start of notes of type "description" -->
        
        <its:locNoteRule selector="//xlf:file//::xlf:source | //xlf:file[not(@appliesTo)]//::xlf:target" localizationNotePointer="ancestor::xlf:file[not(@appliesTo)]/xlf:notes/xlf:note" locNoteType="description"/>
        
        <its:locNoteRule selector="//xlf:group//::xlf:source | //xlf:group[not(@appliesTo)]//::xlf:target" localizationNotePointer="ancestor::xlf:group[not(@appliesTo)]/xlf:notes/xlf:note" locNoteType="description"/>
        
        <its:locNoteRule selector="//xlf:unit[@appliesTo='target']//::xlf:target" localizationNotePointer="ancestor::xlf:unit[@appliesTo='target']/xlf:notes/xlf:note" locNoteType="description"/>
        
        <its:locNoteRule selector="//xlf:file[@appliesTo='target']//::xlf:target" localizationNotePointer="ancestor::xlf:file[@appliesTo='target']/xlf:notes/xlf:note" locNoteType="description"/>
        
        <its:locNoteRule selector="//xlf:group[@appliesTo='target']//::xlf:target" localizationNotePointer="ancestor::xlf:group[@appliesTo='target']/xlf:notes/xlf:note" locNoteType="description"/>
        
        <its:locNoteRule selector="//xlf:unit[@appliesTo='source']//::xlf:source" localizationNotePointer="ancestor::xlf:unit[@appliesTo='source']/xlf:notes/xlf:note" locNoteType="description"/>
        
        <its:locNoteRule selector="//xlf:file[@appliesTo='source']//::xlf:source" localizationNotePointer="ancestor::xlf:file[@appliesTo='source']/xlf:notes/xlf:note" locNoteType="description"/>
        
        <its:locNoteRule selector="//xlf:group[@appliesTo='source']//::xlf:source" localizationNotePointer="ancestor::xlf:group[@appliesTo='source']/xlf:notes/xlf:note" locNoteType="description"/>        
        
        <!-- Note of type "description" -->
        
        <its:locNoteRule selector="//xlf:*[@annotation='comment' and @value]" localizationNotePointer="@value" locNoteType="description"/>
        
        <!-- Rules for Preserve Space: cannot be written since there are not pointer attributes for Preserve Space. But this limitation is OK since a processor can work with this data category locally.-->
        
        <!-- Rules for Translate -->
        <its:translateRule selector="//xlf:*[@translate='no']" translate='no'/>
        <its:translateRule selector="//xlf:*[@translate='yes']" translate='yes'/>
        
        <!-- Rules for External Resource -->
        <its:externalResourceRefRule selector="//res:source | //res:target" externalResourceRefPointer="@href"/>
        
        <!-- Rules for Language Information -->
        <its:langRule selector="//xlf:*[@xml:lang]" langPointer="@xml:lang"/>
        <its:langRule selector="//xlf:*[@itsm:lang]" langPointer="@itsm:lang"/>
        
        <!-- Rules for MT Confidence: cannot be written since there is not pointer attribute for MT Confidence. But this limitation is OK since a processor can work with this data category locally. -->
        
        <!-- Rules for Provenance: cannot be written since there are no pointer attributes for the provenance records written in  XLIFF. But this limitation is OK since a processor can work with this data category locally. -->
        
        <!-- Rules for Terminology -->
        <its:termRule selector="//xlf:mrk[@type='term' and @ref]" termInfoRefPointer="@ref"/>
        
        <!-- Rules for Directionality: This data category is not mapped to XLIFF 2.X -->
        
        <!-- Rules for Elements Within Text: This data category is not represented as metadata in XLIFF 2.X -->
        
        <!-- Rules for ID Value: This data category is not represented as metadata in XLIFF 2.X -->
        
        <!-- Rules for Locale Filter: cannot be written since there are no pointer attributes for locale filter written in  XLIFF. But this limitation is OK since a processor can work with this data category locally. -->
        
        <!-- Rules for Target Pointer -->
        <its:targetPointerRule selector="//xlf:source" targetPointer="../xlf:target"/>
        
        <!-- Rules for storage size: The data category is not covered by the XLIFF 2.1 specification, so no rule is provided. -->
        
    </its:rules>
    <!-- End of ITS Rules -->
    <!-- Removed CTR -->
    <!--<sch:pattern>
        <sch:rule context="ctr:revision">
            <sch:report test="@its:*[not(name()='its:toolRef')][not(name()='its:tool')][not(name()='its:revToolRef')][not(name()='its:revTool')]
                [not(name()='its:revPersonRef')][not(name()='its:revPerson')][not(name()='its:revOrgRef')][not(name()='its:revOrg')]
                [not(name()='its:org')][not(name()='its:provenanceRecordsRef')][not(name()='its:personRef')][not(name()='its:person')]
                [not(name()='its:orgRef')][not(name()='its:annotatorsRef')]">
                Invalid 'itsm' attribute used in 'revision'.
            </sch:report>
        </sch:rule>
    </sch:pattern>-->
    <sch:pattern>
        <sch:rule context="xlf:mrk[@its:annotatorsRef] | xlf:sm[@its:annotatorsRef]">
            <sch:assert test="not(@type) or @type='its:generic'">
                When the 'its:annotatorsRef' attribute is used, the optional 'type' attribute must be set to 'its:generic' if declared.
            </sch:assert>
        </sch:rule>
    </sch:pattern>
    <sch:pattern>
        <sch:rule context="xlf:mrk[@its:locQualityIssuesRef] | xlf:sm[@its:locQualityIssuesRef]">
            <sch:assert test="not(@type) or @type='its:generic'">
                When used in ITS Localization Quality Issue Annotation, the optional 'type' attribute must be set to 'its:generic' if declared.
            </sch:assert>
            <sch:report test="@its:locQualityIssueType or @its:locQualityIssueComment">
                When the 'its:locQualityIssuesRef' attribute is used, the following attributes must be declared: 'its:locQualityIssueType' and 'its:locQualityIssueComment'.
            </sch:report>
            <sch:report test="@its:locQualityIssueSeverity or @its:locQualityIssueProfileRef or @its:locQualityIssueEnabled">
                When the 'its:locQualityIssuesRef' attribute is used, the following attributes must be declared: 'its:locQualityIssueSeverity', 'its:locQualityIssueProfileRef' and 'its:locQualityIssueEnabled'..
            </sch:report>
        </sch:rule>
    </sch:pattern>
    <sch:pattern>
        <sch:rule context="xlf:mrk[@its:locQualityIssueSeverity] | xlf:mrk[@its:locQualityIssueProfileRef] | 
            xlf:sm[@its:locQualityIssueSeverity] | xlf:sm[@its:locQualityIssueProfileRef]">
            <sch:report test="@its:locQualityIssuesRef">
                When the 'its:locQualityIssueSeverity' or 'its:locQualityIssueProfileRef' attributes are used, the 'its:locQualityIssuesRef' must not be declared.
            </sch:report>
        </sch:rule>
    </sch:pattern>
    <sch:pattern>
        <sch:rule context="xlf:mrk[@its:locQualityRatingScore] | xlf:sm[@its:locQualityRatingScore]">
            <sch:report test="@its:locQualityRatingVote">
                When the 'its:locQualityRatingScore' attribute is used, the 'itms:locQualityRatingVote' attribute is not allowed.
            </sch:report>
            <sch:assert test="not(@type) or @type='its:generic'">
                When the 'its:locQualityRatingScore' attribute is used, the optional 'type' attribute must be set to 'its:generic' if declared.
            </sch:assert>
        </sch:rule>
    </sch:pattern>
    <sch:pattern>
        <sch:rule context="xlf:mrk[@its:locQualityRatingVote] | xlf:sm[@its:locQualityRatingVote]">
            <sch:report test="@its:locQualityRatingScore">
                When the 'its:locQualityRatingVote' attribute is used, the 'itms:locQualityRatingScore' attribute is not allowed.
            </sch:report>
            <sch:assert test="not(@type) or @type='its:generic'">
                When the 'its:locQualityRatingVote' attribute is used, the optional 'type' attribute must be set to 'its:generic' if declared.
            </sch:assert>
        </sch:rule>
    </sch:pattern>
    <sch:pattern>
        <sch:rule context="xlf:*[@its:locQualityRatingScoreThreshold]">
            <sch:assert test="@its:locQualityRatingScore or ancestor::xlf:*[@its:locQualityRatingScore]">
                The 'its:locQualityRatingScoreThreshold' attribute may be set only if 'its:locQualityRatingScore' is declared or inherited from upper levels.
            </sch:assert>
        </sch:rule>
    </sch:pattern>
    <sch:pattern>
        <sch:rule context="xlf:*[@its:locQualityRatingVoteThreshold]">
            <sch:assert test="@its:locQualityRatingVote or ancestor::xlf:*[@its:locQualityRatingVote]">
                The 'its:locQualityRatingVoteThreshold' attribute may be set only if 'its:locQualityRatingVote' is declared or inherited from upper levels.
            </sch:assert>
        </sch:rule>
    </sch:pattern>
    <sch:pattern>
        <sch:rule context="xlf:mrk[@its:taIdentRef] | xlf:sm[@its:taIdentRef]">
            <sch:report test="@its:taSource or @its:taIdent">
                When the 'its:taIdentRef' attribute is used, the following attributes are not allowed: 'its:taSource' and 'its:taIdent'.
            </sch:report>
            <sch:assert test="not(@type) or @type='its:generic'">
                When the 'its:taIdentRef' attribute is used, the optional 'type' attribute must be set to 'its:generic' if declared.
            </sch:assert>
        </sch:rule>
    </sch:pattern>
    <sch:pattern>
        <sch:rule context="xlf:mrk[@its:taSource] | xlf:sm[@its:taSource] |
            xlf:mrk[@its:taIdent] | xlf:sm[@its:taIdent]">
            <sch:report test="@its:taIdentRef">
                When the 'its:taSource' or 'its:taIdent' attributes are used, the 'its:taIdentRef' attribute is not allowed.
            </sch:report>
            <sch:assert test="@its:taSource and @its:taIdent">
                The pair of 'its:taSource' and 'its:taIdent'attributes must be present at the same time.
            </sch:assert>
            <sch:assert test="not(@type) or @type='its:generic'">
                When the 'its:taSource' and 'its:taIdent' attributes are used, the optional 'type' attribute must be set to 'its:generic' if declared.
            </sch:assert>
        </sch:rule>
    </sch:pattern>
    <sch:pattern>
        <sch:rule context="xlf:mrk[@its:taClassRef] | xlf:sm[@its:taClassRef]">
            <sch:assert test="not(@type) or @type='its:generic'">
                When the 'its:taClassRef' attribute is used, the optional 'type' attribute must be set to 'its:generic' if declared.
            </sch:assert>
        </sch:rule>
    </sch:pattern>
    <sch:pattern>
        <sch:rule context="xlf:mrk[@its:lang] | xlf:sm[@its:lang]">
            <sch:assert test="not(@type) or @type='its:generic'">
                When the 'its:lang' attribute is used, the optional 'type' attribute must be set to 'its:generic' if declared.
            </sch:assert>
        </sch:rule>
    </sch:pattern>
    <sch:pattern>
        <sch:rule context="xlf:mrk[@its:mtConfidence] | xlf:sm[@its:mtConfidence]">
            <sch:assert test="not(@type) or @type='its:generic'">
                When the 'its:mtConfidence' attribute is used, the optional 'type' attribute must be set to 'its:generic' if declared.
            </sch:assert>
        </sch:rule>
    </sch:pattern>
    
    <sch:pattern>
        <sch:rule context="xlf:mrk[@its:provenanceRecordsRef] | xlf:sm[@its:provenanceRecordsRef]">
            <sch:assert test="not(@type) or @type='its:generic'">
                When the 'its:provenanceRecordsRef' attribute is used, the optional 'type' attribute must be set to 'its:generic' if declared.
            </sch:assert>
            <sch:report test="@its:org or @its:orgRef or @its:person or 
                @its:personRef or @its:revOrg or @its:revOrgRef or 
                @its:revPerson or @its:revPersonRef or @its:revTool or
                @its:revToolRef or @its:tool or @its:toolRef">
                When the 'its:provenanceRecordsRef' attribute is used, the following attributes are not allowed: its:org, its:orgRef, its:person, its:personRef, its:revOrg,
                its:revOrgRef, its:revPerson, its:revPersonRef, its:revTool, its:revToolRef, its:tool and its:toolRef.
                
            </sch:report>
        </sch:rule>
    </sch:pattern>
    <sch:pattern>
        <sch:rule context="xlf:mrk[@its:allowedCharacters] | xlf:mrk[@itsm:domains] | xlf:mrk[@itsm:lang] | xlf:mrk[@its:localeFilterList] |
            xlf:sm[@its:allowedCharacters] | xlf:sm[@itsm:domains] | xlf:sm[@itsm:lang] | xlf:sm[@its:localeFilterList]">
            <sch:assert test="not(@type) or @type='its:generic'">
                When the 'its:allowedCharacters', 'itsm:domains' or 'its:localeFilterList attributes are used, the value of optional 'type' attribute must be set to 'its:generic'.
            </sch:assert>
        </sch:rule>
    </sch:pattern>
    <sch:pattern>
        <sch:rule context="xlf:mrk[@its:locQualityIssuesRef] | xlf:sm[@its:locQualityIssuesRef]">
            <sch:report test="@its:locQualityIssueSeverity or @its:locQualityIssueProfileRef or @its:locQualityIssueEnabled">
                If the 'its:locQualityIssuesRef' attribute is declared, the following attributes are not allowd: its:locQualityIssueSeverity, its:locQualityIssueProfileRef, and its:locQualityIssueEnabled".
            </sch:report>
            <sch:assert test="count(ancestor::xlf:unit//its:locQualityIssues[@xml:id=substring-after(current()/@its:locQualityIssuesRef , '#its=')])=1">
                The value of the locQualityIssuesRef attribute must be relative URI (starting with "#its="), and the value after "#its=" must be one of the id attributes declared on a &lt;locQualityIssues> elements within the same 'unit'.
            </sch:assert>
        </sch:rule>
    </sch:pattern>
    <sch:pattern>
        <sch:rule context="xlf:mrk[@its:locQualityIssuesRef] | xlf:sm[@its:locQualityIssuesRef]">
            <sch:report test="@its:locQualityIssueType or @its:locQualityIssueComment">
                When 'its:locQualityIssuesRef' is declared, 'its:locQualityIssueType' and its:locQualityIssueComment are not allowed.
            </sch:report>
        </sch:rule>
    </sch:pattern>
    <sch:pattern>
        <sch:rule context="xlf:mrk[@its:locQualityIssueType] | xlf:mrk[@its:locQualityIssueComment] |
                           xlf:sm[@its:locQualityIssueType] | xlf:sm[@its:locQualityIssueComment] ">
            <sch:report test="@its:locQualityIssuesRef">
                When 'its:locQualityIssueType' or 'its:locQualityIssueComment' are declared, 'its:locQualityIssuesRef' is not allowed.
            </sch:report>
        </sch:rule>
    </sch:pattern>
   <sch:pattern>
        <sch:rule context="xlf:*[@its:annotatorsRef][not(contains(@its:annotatorsRef, ' '))]">
            <sch:let name="ref" value="@its:annotatorsRef"/>
            <sch:let name="its-dc-id" value="substring-before($ref,'|')"/>
            <sch:report test="$its-dc-id!='allowed-characters' and $its-dc-id!='directionality' and $its-dc-id!='domain' and $its-dc-id!='elements-within-text' and
                $its-dc-id!='external-resource' and $its-dc-id!='id-value' and $its-dc-id!='language-information' and
                $its-dc-id!='locale-filter' and $its-dc-id!='localization-note' and $its-dc-id!='localization-quality-issue' and
                $its-dc-id!='localization-quality-rating' and $its-dc-id!='mt-confidence' and $its-dc-id!='preserve-space' and
                $its-dc-id!='provenance' and $its-dc-id!='storage-size' and $its-dc-id!='target-pointer' and
                $its-dc-id!='terminology' and $its-dc-id!='text-analysis' and $its-dc-id!='translate'">
                Invalid id used for the ITS datacategory <sch:value-of select="$its-dc-id"/>. Please see the Specification for guidelines on the value of this attribute.
            </sch:report>
        </sch:rule>
    </sch:pattern>
    <sch:pattern>
        <sch:rule context="xlf:*[@its:annotatorsRef][(contains(@its:annotatorsRef, ' '))]">
            <sch:let name="ids-string" value=" replace(@its:annotatorsRef, '\s+',' ')"/>
            <sch:report  test="contains(substring-after($ids-string,'allowed-characters'),'allowed-characters')  or contains(substring-after($ids-string,'directionality'),'directionality') or contains(substring-after($ids-string,'elements-within-text'),'elements-within-text') or contains(substring-after($ids-string,'external-resource'),'external-resource') or contains(substring-after($ids-string,'id-value'),'id-value') or contains(substring-after($ids-string,'language-information'),'language-information') or contains(substring-after($ids-string,'locale-filter'),'locale-filter') or contains(substring-after($ids-string,'localization-note'),'localization-note') or contains(substring-after($ids-string,'localization-quality-issue'),'localization-quality-issue') or contains(substring-after($ids-string,'localization-quality-rating'),'localization-quality-rating') or contains(substring-after($ids-string,'mt-confidence'),'mt-confidence') or contains(substring-after($ids-string,'preserve-space'),'preserve-space') or contains(substring-after($ids-string,'provenance'),'provenance') or contains(substring-after($ids-string,'storage-size'),'storage-size') or contains(substring-after($ids-string,'target-pointer'),'target-pointer') or contains(substring-after($ids-string,'terminology'),'terminology') or contains(substring-after($ids-string,'text-analysis'),'text-analysis') or contains(substring-after($ids-string,'translate'),'translate')"> 
                Each ITS data category identifier must not be used more than once.
            </sch:report>
         </sch:rule>
    </sch:pattern>
    <sch:pattern>
        <sch:rule context="its:locQualityIssue">
            <sch:assert test="@locQualityIssueType or @locQualityIssueComment">
                At least one of the attributes locQualityIssueType or locQualityIssueComment must be set.
            </sch:assert>
        </sch:rule>
    </sch:pattern>
    <sch:pattern>
        <sch:rule context="its:provenanceRecord">
            <sch:assert test="@org or @orgRef or @person or 
                @personRef or @revOrg or @revOrgRef or 
                @revPerson or @revPersonRef or @revTool or
                @revToolRef or @tool or @toolRef">
                At least one of the followings must be set: its:org, its:orgRef, its:person, its:personRef, its:revOrg,
                its:revOrgRef, its:revPerson, its:revPersonRef, its:revTool, its:revToolRef, its:tool and its:toolRef.
            </sch:assert>
        </sch:rule>
    </sch:pattern>
    <sch:pattern>
        <sch:rule context="its:locQualityIssues | its:provenanceRecords[ancestor::xlf:unit]">
            <sch:let name="id" value="@xml:id"/>
            <sch:let name="counter" value="count(ancestor::xlf:unit//its:locQualityIssues[@xml:id=$id] | ancestor::xlf:unit//its:provenanceRecords[@xml:id=$id])"/>
            <sch:assert test="$counter=1">
                Value of 'id' must be unique among all of  &lt;locQualityIssues> and &lt;provenanceRecords> elements within the enclosing 'unit'.
            </sch:assert>
        </sch:rule>
    </sch:pattern>
    <sch:pattern>
        <sch:rule context="its:provenanceRecords[ancestor::xlf:group][not(ancestor::xlf:unit)]">
            <sch:let name="id" value="@id"/>
            <sch:let name="counter" value="count(ancestor::xlf:group[1]//its:provenanceRecords[@id=$id][not(ancestor::xlf:unit)])"/>
            <sch:assert test="$counter=1">
                Value of 'id' must be unique among all of  &lt;provenanceRecords> elements within the enclosing 'group'.
            </sch:assert>
        </sch:rule>
    </sch:pattern>
    <sch:pattern>
        <sch:rule context="its:provenanceRecords[ancestor::xlf:file][not(ancestor::xlf:unit)][not(ancestor::xlf:group)]">
            <sch:let name="id" value="@id"/>
            <sch:let name="counter" value="count(ancestor::xlf:file//its:provenanceRecords[@id=$id][not(ancestor::xlf:group)][not(ancestor::xlf:unit)])"/>
            <sch:assert test="$counter=1">
                Value of 'id' must be unique among all of  &lt;provenanceRecords> elements within the enclosing 'file'.
            </sch:assert>
        </sch:rule>
    </sch:pattern>
    <sch:pattern>
        <sch:rule context="xlf:*[@its:locQualityIssuesRef]">
            <sch:assert test="not(@its:locQualityIssueSeverity)">
                If 'its:locQualityIssuesRef' attribute is declared, the 'its:locQualityIssueSeverity' must not be used. 
            </sch:assert>
            <sch:assert test="not (@its:locQualityIssueProfileRef) ">
                If 'its:locQualityIssuesRef' attribute is declared, the 'its:locQualityIssueProfileRef' must not be used. 
            </sch:assert>
            <sch:assert test="not(@its:locQualityIssueEnabled)">
                If 'its:locQualityIssuesRef' attribute is declared, the 'its:locQualityIssueEnabled' must not be used. 
            </sch:assert>
        </sch:rule>        
    </sch:pattern>
</sch:schema>
