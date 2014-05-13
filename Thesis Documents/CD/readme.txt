# # # # # # # # # # # # # # # # # # # # # # # # # # # 

Graphical User Interface (GUI) for bark (console application).
Developed by Giannis Chantzis in cooperation with Onoufrios Haralampous as a Thesis for TEI Thessalias (2014).

This application generates & modifies XML files with data destined to be used in bark for calculations.

# # # # # # # # # # # # # # # # # # # # # # # # # # # 

Requires:
	Bark.exe software
	Library "ZedGraph.dll"
	Samples folder containing:
		an empty XML file template named "NewXmlFile.brk"
		Xml schema validator named "XSDValidator.xsd"
		the parts of the Xml schema validator:
			"XSDValidatorComplexTypes.xsd"
			"XSDValidatorSimpleTypes.xsd"
			"XSDValidatorFunctions.xsd"
			"XSDValidatorUnits.xsd"


# # # # # # # # # # # # # # # # # # # # # # # # # # # 

Languages used:		C#, XML, XSD, UML
Main program written in:	C#
Tools used:		Microsoft Visual Studio Express 2012 - Visual C# 2012, GitHub (Version Control),
			Notepad++ with plug-in XML Tools (XML Syntax Check & Validation against XSDs),
			ArgoUML (Visual representation of C# Classes)
Tested in Operating Systems:	Windows 7 Professional SP1
External libraries:		ZedGraph (implementation of charts and graphs for visual data representation)

# # # # # # # # # # # # # # # # # # # # # # # # # # # 

Notes:
	If you're using the installer, always run the executable file with administrative rights,
		in order to gain write access in the ProgramFiles folder.
	If any file cannot be opened edit the filepaths using GUI window preferences.
	Comments in the code in this application are written in English.