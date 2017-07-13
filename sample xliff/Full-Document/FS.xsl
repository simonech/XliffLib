<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
  xmlns:xs="http://www.w3.org/2001/XMLSchema"
  exclude-result-prefixes="xs"
  version="2.0">
  
  <xsl:template match="*" priority="2"
    xmlns:fs="urn:oasis:names:tc:xliff:fs:2.0">
    <xsl:choose>
      <xsl:when test="@fs:fs">
        <xsl:element name="{@fs:fs}">
          <xsl:if test="@fs:subFs">
            <xsl:variable name="att_name"
              select="substring-before(@fs:subFs,',')" />
            <xsl:variable name="att_val"
              select="substring-after(@fs:subFs,',')" />
            <xsl:attribute name="{$att_name}">
              <xsl:value-of select="$att_val" />
            </xsl:attribute>
          </xsl:if>
          <xsl:apply-templates />
        </xsl:element>
      </xsl:when>
      <xsl:otherwise>
        <xsl:apply-templates />
      </xsl:otherwise>
    </xsl:choose>
  </xsl:template>
  
</xsl:stylesheet>