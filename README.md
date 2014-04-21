#bark_GUI#
----------
#bark_GUI TODO#
---------------

- [ ] Check/fix dirty files.
- [ ] To add layers μόνο στα layers και το add materials μόνο στα materials.


##PRIMARY TODO##
- [ ] Read from XSD & build acordingly
- [ ] Handle type change (control replace without losing data)
      1. Create Control of that type with the correct data (XSD);
      2. Place it where this control was;
      3. Update the _XmlDocument;
	  4. Remove this (control);
- [ ] Handle Tree structure changes & update _XmlDocument
	Στα materials και στα layers πρέπει να υπάρχει η δυνατότητα για δεξί κλικ και add material ή layer.
	Προφανώς και delete.

##SECONDARY TODO##
- [ ] create 'help', tooltips & stuff via XML
- [ ] try-catch dangerous code (like file handling, Tags, casts, null references)
- [ ] 'Tab' Indexes on controls
- [ ] Preferences> 'directory' search buttons
- [ ] Flow/Resize controls.
- >	Γενικώς είμαι της άποψης, όσο πιο λιτό γίνεται και αν χρειατεί κάτι στην πράξη τότε το προσθέτουμε.
- [ ] Open XML files that are invalid (against an XSD)
- [ ] Το θέμα είναι τι κάνει το GUI από εκεί και πέρα. Γενικώς αν υπάρχει δυνατότητα να διορθωθεί το σφάλμα
	 εντός του GUI, τότε να φορτώσει το αρχείο, αλλιώς δεν το φορτώνει κι υποδεικνύει στον χρήστη
	 να το διορθώσει εξωτερικά (δεν με πειράζει κι αυτό). Να το δούμε στην πράξη.
- [ ] Properly handle the XMLHandler XSD & XML load errors in the try-catch. It doesnt always have to be a messagebox.
- [ ] Θα ήταν πιο βολικό στο variable τα ξεχωριστά κουτάκια να φαίνονται ως πίνακας, έτσι ώστε ο χρήστης να μπορεί να κάνει copy paste μόνο ένα μέρος του πίνακα (π.χ. λίγες γραμμές ή μία στήλη). Επίσης copy paste ορισμένα κελιά από το bark_GUI στο bark_GUI.
- [x] Γενικώς δουλεύει. Όταν όμως το path του αρχείου έχει κενά, μπερδεύεται. Χρειάζεται να του βάλεις εισαγωγικά όταν δώσεις την εντολή εκτέλεσης. Π.χ.
	Bark.exe New Folder/temp.bark              a Νομίζει ότι το αρχείο λέγεται New και βγάζει λάθος.
	Bark.exe New Folder/temp.bark          a Δουλεύει
- [ ] Το scroll bar του variable να έρθει πιο δεξιά, για να ευθυγραμμιστεί η δεύτερη στήλη με τις μονάδες


##FUTURE TODO##
- [ ] Handle Functions (wallsun.brk)
- [ ] Simulate.
- [ ] Perfect UML Diagrams.
- [ ] Add user custom types. (like decimal_positive)

##POLISHING##
- [ ] Element view: Τα optional labels να φαίνονται με διαφορετικό χρώμα.
- [ ] Check XmlNode.FirstChild careless use.



#bark_GUI CHANGELOG#
--------------------

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
