# ![Vintall's Unity Free Online Config](https://github.com/Vintall/Unity-Free-Online-Config/blob/media/Media/Homepage/Logo1StockCompressed.png "VUFOC")
VUFOC is a simple external config's loader, for places, where you need to change a few variables but can't / don't want to make another build.
You can download you config in one press of a button from inspector or download on startup.
Create as many configs as you like

# Features
- Minimalistic and Quick
- No need to host or buy a server
- Unlimited configs

# How to install

## Install via GIT URL

- Copy link to package `https://github.com/Vintall/Unity-Free-Online-Config.git#master`
- In unity go to **Window** -> **Package Manager**

<img src="https://github.com/Vintall/Unity-Free-Online-Config/blob/media/Media/Guideline/Guideline7Compressed.png" alt="Guideline7" width="600"><img/>

- In the top left corner click **+** -> **Add package from git URL...**

<img src="https://github.com/Vintall/Unity-Free-Online-Config/blob/media/Media/Guideline/Guideline8Compressed.png" alt="Guideline8" width="600"><img/>

- Paste the link to your spreadsheet in field and click **Add**.

<img src="https://github.com/Vintall/Unity-Free-Online-Config/blob/media/Media/Guideline/Guideline9Compressed.png" alt="Guideline9" width="600"><img/>

## Install via Unity Package

- Download the latest release https://github.com/Vintall/Unity-Free-Online-Config/releases
- Double-click on .unitypackage or drag it onto editor
- Choose files to import. You will import all files by default

# Quick start

## Create and publish Google spreadsheet
- Go to https://docs.google.com/spreadsheets. Make sure you're logged into your account
- Create a blank spreadsheet.

<img src="https://github.com/Vintall/Unity-Free-Online-Config/blob/media/Media/Guideline/Guideline1Compressed.png" alt="Guideline1" width="600"><img/>

- Go to **File** -> **Share** -> **Publish to web**

<img src="https://github.com/Vintall/Unity-Free-Online-Config/blob/media/Media/Guideline/Guideline2Compressed.png" alt="Guideline2" width="600"><img/>

- In "Publish to the web" pop-up in the left column choose a specific sheet that you want to use as your database. Later on you can make as many as you want.

<img src="https://github.com/Vintall/Unity-Free-Online-Config/blob/media/Media/Guideline/Guideline3Compressed.png" alt="Guideline3" width="600"><img/>

- In the right column pick **Tab-separated values (.tsv)**

<img src="https://github.com/Vintall/Unity-Free-Online-Config/blob/media/Media/Guideline/Guideline4Compressed.png" alt="Guideline4" width="600"><img/>

- In "Published content & settings" you have several fields.
	- First of all, choose entire document in first field. 
	- You most likely want to have enabled "Automatically republish when changes are made". Otherwise you will need to republish it manually every time. 
	- Press **Start publishing** to generate link to your sheet.

<img src="https://github.com/Vintall/Unity-Free-Online-Config/blob/media/Media/Guideline/Guideline5Compressed.png" alt="Guideline5" width="600"><img/>

- Copy the link to your sheet.

<img src="https://github.com/Vintall/Unity-Free-Online-Config/blob/media/Media/Guideline/Guideline6Compressed.png" alt="Guideline6" width="600"><img/>

## Connect to spreadsheet in Unity

- Create two scripts: ExampleDatabase.cs and ExampleVo.cs
- ExampleVo extend class ASpreadsheetVo. This is one line of data from your database.
```
[Serializable]
public class ExampleVo : ASpreadsheetVo
{
    public string ExampleString;
    public int ExampleInt;
    public float ExampleFloat;
    public double ExampleDouble;
}
```
- ExampleDatabase is a class, that holds database name, url and list of ExampleVo's. Every database have a ScriptableObject, as base class. So, for every database we need to specity attribute [CreateAssetMenu]. You can read more about ScriptableObjects in official unity documentation https://docs.unity3d.com/Manual/class-ScriptableObject.html.
```
[CreateAssetMenu(fileName = "ExampleDatabase", menuName = "Databases/ExampleDatabase")]
public class ExampleDatabase : ASpreadsheetDatabase<ExampleVo>
{
    [SerializeField] private string databaseName;
    [SerializeField] private string databaseDataUrl;
    
    public override string DatabaseName => databaseName;
    public override string DatabaseDataUrl => databaseDataUrl;
    protected override ASpreadsheetVo TemplateVo => new ExampleVo();
}
```
- In editor right click onto project window -> Create -> Databases -> ExampleDatabase. That will create a ScriptableObject of your database

<img src="https://github.com/Vintall/Unity-Free-Online-Config/blob/media/Media/Guideline/Guideline10Compressed.png" alt="Guideline10" width="600"><img/>

- Go to your spreadsheet and fill up some data. Columns shoud be corresponding in format an order. In our case ExampleVo has the folowing fields: "string", "int", "float", "double". So, the column will be read as "A":"string", "B":"int", "C":"float", "D":"double".

<img src="https://github.com/Vintall/Unity-Free-Online-Config/blob/media/Media/Guideline/Guideline11Compressed.png" alt="Guideline11" width="600"><img/>

- Go back to editor. And place database name (optional) and url to your shared sheet. Then click *Download*

<img src="https://github.com/Vintall/Unity-Free-Online-Config/blob/media/Media/Guideline/Guideline12Compressed.png" alt="Guideline12" width="600"><img/>

- Data from spreadsheet should appear in *SpreadsheetEntries* list.

<img src="https://github.com/Vintall/Unity-Free-Online-Config/blob/media/Media/Guideline/Guideline13Compressed.png" alt="Guideline13" width="600"><img/>



