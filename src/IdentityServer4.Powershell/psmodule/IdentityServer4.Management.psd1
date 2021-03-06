#
# Modulmanifest für das Modul "IdentityServer4.Management"
#

@{

# Die diesem Manifest zugeordnete Skript- oder Binärmoduldatei.
RootModule = '.\IdentityServer4.Powershell.dll'

# Die Versionsnummer dieses Moduls
ModuleVersion = '2.1.0'

# ID zur eindeutigen Kennzeichnung dieses Moduls
GUID = 'a346ddc8-0793-46b8-a592-edf6984812bc'

# Autor dieses Moduls
Author = 'Thomas Glatzer'

# Unternehmen oder Hersteller dieses Moduls
CompanyName = 'ZDV - Johannes Gutenberg Universität-Mainz'

# Urheberrechtserklärung für dieses Modul
Copyright = '(c) 2018 Thomas Glatzer. Alle Rechte vorbehalten.'

# Beschreibung der von diesem Modul bereitgestellten Funktionen
Description = 'Powershell Modul für das Management von IdentityServer4 (https://identityserver.io/) mit EntityFramework. Ermöglicht das Verwalten von Clients und API-Resourcen.'

# Aus diesem Modul zu exportierende Cmdlets. Um optimale Leistung zu erzielen, verwenden Sie keine Platzhalter und löschen den Eintrag nicht. Verwenden Sie ein leeres Array, wenn keine zu exportierenden Cmdlets vorhanden sind.
CmdletsToExport = '*'

# Die privaten Daten, die an das in "RootModule/ModuleToProcess" angegebene Modul übergeben werden sollen. Diese können auch eine PSData-Hashtabelle mit zusätzlichen von PowerShell verwendeten Modulmetadaten enthalten.
PrivateData = @{

    PSData = @{

        # 'Tags' wurde auf das Modul angewendet und unterstützt die Modulermittlung in Onlinekatalogen.
        Tags = @('IdentityServer', 'IdentityServer4', 'IdentityServer.EntityFramework', 'Open ID Connect', 'OIDC')

        # Eine URL zur Lizenz für dieses Modul.
        # LicenseUri = ''

        # Eine URL zur Hauptwebsite für dieses Projekt.
        ProjectUri = 'https://github.com/jguzdv/IdentityServer4.Powershell'

        # Eine URL zu einem Symbol, das das Modul darstellt.
        # IconUri = ''

        # 'ReleaseNotes' des Moduls
        # ReleaseNotes = ''

    } # Ende der PSData-Hashtabelle

} # Ende der PrivateData-Hashtabelle

# HelpInfo-URI dieses Moduls
# HelpInfoURI = ''

# Standardpräfix für Befehle, die aus diesem Modul exportiert werden. Das Standardpräfix kann mit "Import-Module -Prefix" überschrieben werden.
DefaultCommandPrefix = 'IdSrv'

}

