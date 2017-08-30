<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:msxsl="urn:schemas-microsoft-com:xslt" >
  <xsl:output method="html" indent="no"/>
  <xsl:include href="/_layouts/xsl/main.xsl"/>
  <xsl:include href="/_layouts/xsl/internal.xsl"/>
  <xsl:template match="FieldRef" mode="header">
    <xsl:apply-templates />
  </xsl:template>
  <xsl:template match="View[@BaseViewID='2']" mode="full">
    <xsl:copy-of select="*"/>
  </xsl:template>
</xsl:stylesheet>