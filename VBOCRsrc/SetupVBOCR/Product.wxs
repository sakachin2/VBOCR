<?xml version="1.0" encoding="utf-8"?>
<!--  codepage,65001:utf8, 932:shift_jis, 28591:iso8859-1; 1252:ansi latin1   LCID 1041:ja, 1033:en-us            -->
<Wix xmlns="http://schemas.microsoft.com/wix/2006/wi">
<!--
    <Product Id="B2C2974C-FA50-47D6-B514-31588DB39284" Name="sampleVBOCR" Language="1033" Version="1.0.2" Manufacturer="sakachin2" UpgradeCode="5DFB9A2C-9768-42E2-A3FF-512821010F62">
-->
    <Product Id="*" Name="sampleVBOCR" Language="1033" Version="1.0.2" Manufacturer="sakachin2" UpgradeCode="5DFB9A2C-9768-42E2-A3FF-512821010F62">
        <Package Description="Extract text in image file" Comments="OCR tool sampleVBOCR installer" InstallerVersion="200" Compressed="yes" />
        <Media Id="1" Cabinet="simple.cab" EmbedCab="yes" />
        <Directory Id="TARGETDIR" Name="SourceDir">
            <Directory Id="ProgramFilesFolder" Name="PFiles">
                <!--  name:install foldername -->
                <Directory Id="VBOCR" Name="sampleVBOCR">
                    <Directory Id="RELEASE" Name="Release">
                        <Component Id="VBOCR.EXE" DiskId="1" Guid="CD3DB34A-35BC-45A4-A023-1CBE2A1A6D14">
                            <File Id="VBOCR.EXE" Name="VBOCR.exe" Source="W:\MSVS2017CProjects\VBOCR\VBOCR\bin\Release\sampleVBOCR.exe" />
                        </Component>
                        <Component Id="VBOCR.EXE.CONFIG" DiskId="1" Guid="2D3028BD-F7DE-4440-977C-B6A5EF068A81">
                            <File Id="VBOCR.EXE.CONFIG" Name="VBOCR.exe.config" Source="W:\MSVS2017CProjects\VBOCR\VBOCR\bin\Release\sampleVBOCR.exe.config" />
                        </Component>
                        <Component Id="VBOCR.XML" DiskId="1" Guid="192C8444-8CAC-4B7F-A227-68232653F5C2">
                            <File Id="VBOCR.XML" Name="VBOCR.xml" Source="W:\MSVS2017CProjects\VBOCR\VBOCR\bin\Release\sampleVBOCR.xml" />
                        </Component>
                    </Directory>
                </Directory>
            </Directory>
        </Directory>
        <Feature Id="DefaultFeature" Title="Main Feature" Level="1">
            <ComponentRef Id="VBOCR.EXE" />
            <ComponentRef Id="VBOCR.EXE.CONFIG" />
            <ComponentRef Id="VBOCR.XML" />
        </Feature>
        <UI />
        <UIRef Id="WixUI_InstallDir" />
        <!--  value: id ofDirectory Entry to be installer(Line.9)  -->
        <Property Id="WIXUI_INSTALLDIR" Value="VBOCR" />
        <WixVariable Id="WixUILicenseRtf" Value="license-mit.rtf" />
    </Product>
</Wix>