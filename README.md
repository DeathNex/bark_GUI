#bark_GUI#
----------
Graphical User Interface (GUI) for bark (console application).
Developed by Giannis Chantzis in cooperation with Onoufrios Haralampous as a Thesis for TEI Larisas (2014).

This application generates & modifies XML files with data destined to be used in bark for calculations.


Languages used:		C#, XML, XSD, UML
Main program written in:	C#
Tools used:		Microsoft Visual Studio Express 2012 - Visual C# 2012, Github (Version Control),
			Notepad++ with plugin XML Tools (XML Syntax Check & Validation against XSDs),
			ArgoUML (Visual representation of C# Classes)
Tested in Operating Systems:	Windows 7 Proffesional SP1
External libraries:		ZedGraph (implementation of charts and graphs for visual data representation)

Note: Comments in the code in this application are written in English.



#bark_GUI CHANGELOG#
--------------------

v1.00
-----
+	Fixed a 'SaveChanges' dialog minor bug.
+	Added Status message for control drawing and layout.
+	Moved ZedGraph library inside the project folder.
+	Cleaned up project folder.

v0.23
-----
+	Removed Functions from XSDValidatorComplexTypes and from all XML files.

v0.22
-----
+	Made proper use of status messages for user feedback in case of slow machines or problems.
+	Fixed a bug with controls not validating correctly in a specified case.
+	Removed 'Graph' element from the XSD and XML files.

v0.21
-----
+	Replaced custom control variable table with DataGridView due to error when loading a lot of variable data.
+	Added scroll to current control in element viewer when a type change occurs.
+	Updated and expanded UML diagrams.
+	Prepared screenshots for thesis theory.
+	Fixed a minor bug with Simulation Start.

v0.20
-----
+	Implemented new file using an empty XML template. (Cannot dynamically create a new xml file from xsd)
+	Fixed Right-Click action 'Add' and removed 'Duplicate'.
+	Empty element items are not created in the xml file.
+	Added validation of xml values before saving.
+	Changed 'coordinates' child elements in XSD to be required if the parent element exists.
+	Reworked 'About'.
+	Fixed Dirty File checking for save.
+	Fixed minor SaveAs dialog issues.
+	Added focus textbox on item name/rename.
+	Fixed a bug on rename-cancel.
+	Generated XML is now indented with tabs instead of 2 spaces.
+	Added restriction on multiple items, at least one must exist. (fixes the parent 'Add' problem too)
+	Added logic on xml creation. If a group item is empty, don't create it at all.
+	Added logic on item values validation. If a group item is not required and
		all it's children are empty, the group item valid.
+	Added Simulation>Create Graph action in the main menu. (Loaded xml file name with .dat extension inside Samples)
+	Added last simulation time (last time modified .dat file) in Axis' selection dialog.
+	Implemented move node (drag 'n drop) on tree viewer (with right-click actions).
+	Automatically zooms out on diagram creation.
+	Added XML file validation before save.


v0.19
-----
+	Added NewName user input validation. The NewName must be unique.
+	Fixed 'Save' data to XML.
+	Removed XmlNode from project.
+	Implemented ConvertToXml(Item) method in XmlParser.
+	Fixed reference lists updating on controls.
+	Saving handles properly Units, XUnits, References and Keywords.
+	Fixed Variable Table GetValue bugs.
+	Fixed some broken links.
+	Code cleanup.

v0.18
-----
+	Split Structure from Items with data. Structure now contains items' structure without XmlNodes.
		One DataRootItem created. Updated the whole project to work with this change. (bugs still exist)
+	Created .dat file DataParser.
+	Created data Titles Selection Form/Dialog for axis' from .dat files before the chart creation.
+	Loading properly the data from .dat files and graphical representation of these data
		in a chart by ZedGraph. Limited to 10 Y-Axis' values.
+	Proper error handling in each case of invalid data.
+	Updated ZedGraph library to the latest build (5.15).
+	Removed 4th number from Assembly Version. (Menu>About)


v0.17
-----
+	User input validation.
+	Variable table supression/deletion of empty rows fixed.
+	Saving viewer separator's position and fixed to tree viewer.
+	Changed Optional group items are shown without bold and required group items with bold.
+	Changed element viewer to always show required items, even if they don't have a value. (except empty group items)

v0.16
-----
+	Implemented the Right-Click actions (Add/Duplicate/Rename/Delete) on the TreeViewer. (with bugs)
+	Implemented the Right-Click action (Add) for parents on the TreeViewer.
		Condition for right-click on parent: First child that can have right-click actions.
+	Fixed a visual bug on empty categories.
+	General improvements.

v0.15
-----
+	Implemented the 'New' (Xml file) of the menu.
+	Prepared right-click actions to work on specific treeViewer elements.
		Requirement for right-click: (MinOccurs=0 & MaxOccurs="unbounded")
+	Added 'MinOccurs=0' in the material element inside the XSDValidator.xsd,
		to allow right-click on all multiple-infinite elements.
+	Made use of 'default' values from XSD.
+	Renamed the 'Show all optional items' to 'Show all items'.
+	Updated the 'Show all items' to ignore default values.
+	Refactored class ElementItem & added error handling.
+	Refactored XSD Handler.
+	General Refactor.

v0.14b
-----
+	Fixed a bug with the 'Show all optional items'.
+	Changed the rules of 'optional items' to show an item anyway if it has a value.
+	Empty GroupItems are now hidden. (unless you select an empty & optional child GroupItem)
+	Fixed TreeViewer to show the CustomNames instead of the XML Element names. (e.g. polysterene)

v0.14
-----
+	Fixed 'Multiple' elements overwriting each other.
+	Renamed the "XSDValidatorUtility.xsd" to "XSDValidatorSimpleTypes.xsd".
+	Implemented 'Keyword' element. Changed the "XSDValidator.xsd", "XSDValidatorComplexTypes.xsd",
		"XSDValidatorSimpleTypes" and all XML (.brk) files to work with the new structure.
+	Implemented 'Reference' element. Applies to every element from the XSD that
		is under key-keyref restriction. Uses the name of the element to create the list
		(e.g. all 'material' elements).
+	Added controls to all 'Group Elements' for better handling in the Element Viewer.
+	Fixed the order of the Element Viewer. (Multiple elements no longer appear in the bottom)
+	Visual Update on Element Viewer to help distinguish Grouped/Nested Element Items.

v0.13
-----
+	Added to GitHub.
+	Fixed Recent List not being saved properly.

v0.12
-----
+	General optimization.
+	Reworked ElementTypes.
+	Changed in XSDValidatorFunctions.xsd, XSDValidatorUtility.xsd and XSDValidatorComplexTypes.xsd
		the <keyword> from simpleType to complexType.
+	Enabled Keywords. (can't be viewed 'cause it exists only inside a function)
+	Changed in XSDValidator.xsd, and XSDValidatorComplexTypes.xsd
		the <material>'s complexType moved to XSDValidatorComplexTypes,xsd.
+	Changed in XSDValidator.xsd
		the maxOccurs attribute moved from the <choice> to the appropriate <element>.
+	Updated polysterine.brk to the 0.5.2 bark version.

v0.11
-----
+	Load references from XSD.
+	Reworked & optimized custom_controls code by adding General_Control.
+	Added help to group items as well. (TreeViewer)
+	Optimized code by creating class XSD_Parser, seperating XSD from other functions.
+	Optimized code. (general)

v0.10
-----
+	Created Custom_Control Control_Group. (instead of a label)
+	Created Custom_Control Control_Keyword. (same as reference)
+	Fixed how to check if an item is required or not.
+	Completed the 'Show All Optional Items' checkbox on the treeViewer.
+	Optimized Code.
+	Help text shown when mouse-overing an Element parameter name.
+	Completed switch between constant/variable/function.
+	Reworked custom_controls code.

v0.09
-----
+	Added comments to XSD_Handler.
+	Created an 'Empy Unit' in Units.xsd and used it in ComplexTypes.xsd.
+	General fixes.
+	Changed UML Diagrams.
+	Load Main XSD.
+	Updated 'About Description'.
+	Added quotes (" ") on the filepaths when calling bark.exe.
+	Visually loads units from XSD. (s/min/hour)
+	Visually loads types from XSD. (constant/variable)
+	Allow dropdown lists on element items only when they have more than 1 option.
+	Categorize the element items according to group items in the element viewer.
+	Reconnect tree viewer with element viewer. (change element viewer on tree viewer select category)
+	No visual load from XML yet.

v0.08
-----
+	Copy (from Excel) to variable table. (finished)
+	General Structure reworked. (still working on it)
+	Load ComplexTypes from XSD. (Link with Functions)
+	Structure static & public,
+	Load Functions from XSD.

v0.07
-----
+	Completed About.
+	Updated icon.
+	Preferences window updated.
+	Run bark.exe.
+	XSD updated.
+	Aligned variable/function/reference custom controls.
+	Set minimum variable table to 1.

v0.06
-----
+	Updated XSDs.
+	Icon added.
+	General structure for XML item reworked.
+	Handled a few errors with XSD loading/saving.
+	Created 'About' form.
+	General structure. (still working on it)
+	Load Units from XSD.
+	Load Utility from XSD.
+	ItemType, SimpleType, ComplexType & Unit structures. (still working on them)
+	Updated UML.

v0.05
-----
+	Reversed treeView bold names.
+	Added indent on saved XML files.
+	Load tree from XSD. (broken link with elementViewer)
+	General structure for XML item.
+	Split XSD file to smaller files.
+	Remember last WindowState/Size/Location.
+	Loaded filename moved to title.

v0.04
-----
+	Completed 'Save' & 'Save As' buttons.
+	First check if file exists before loading. If not remove from recent list.
+	Fixed -reference change on runtime- error.
+	Base structure for element type change. (constant/variable/function)
+	Open main window size changed. Min size set.

v0.03
-----
+	Check if elements were changed. (dirty files)
+	Apply element changes on runtime.
x	Bug: If the element's value is deleted the element won't be shown on the viewer.
+	Save XML Document.
+	Special treatment for variable tables on runtime. (fill/cut zeros)
+	Added 'Show Hidden Items' checkbox above treeView. (? No Action)

v0.02
-----
+	ElementViewer shows/hides elements depending on the treeView item selected. (optimized)
+	Status labels. (window bottom)
+	Show element name on tree. (if there is an attribute 'name')
+	Show selected tree item path.
+	General appearance changes.
+	Added Custom Control for 'function' & material 'reference'.
+	Handle 'functions'.
+	Check if tree was changed. (dirty files - undone)
+	Created a base Right-Click on the treeViewer. (No Actions)
+	Split Load treeViewer and elementViewer methods.

v0.01
-----
+	Full XML to tree view. (removed values)
+	Full XML-elements to element view.
+	Filepath as argument available.
+	Custom controls for constant/variable elements.
+	Preferences Updated.
+	UML Diagram.
+	XSD Updated, Material reference key added.
