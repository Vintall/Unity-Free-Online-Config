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

1. Copy link to package `https://github.com/Vintall/Unity-Free-Online-Config.git#master`
2. In unity go to **Window** -> **Package Manager**
<img style = "align: center" src="https://github.com/Vintall/Unity-Free-Online-Config/blob/media/Media/Guideline/Guideline7Compressed.png" alt="Guideline7" width="600"/>
3. In the top left corner click **+** -> **Add package from git URL...**
<img style = "align: center" src="https://github.com/Vintall/Unity-Free-Online-Config/blob/media/Media/Guideline/Guideline8Compressed.png" alt="Guideline8" width="600"/>
4. Paste the link to your spreadsheet in field and click **Add**.
<img style = "align: center" src="https://github.com/Vintall/Unity-Free-Online-Config/blob/media/Media/Guideline/Guideline9Compressed.png" alt="Guideline9" width="600"/>

## Install via Unity Package

1. Download the latest release https://github.com/Vintall/Unity-Free-Online-Config/releases
2. Double-click on .unitypackage or drag it onto editor
3. Choose files to import. You will import all files by default

# Quick start

## Create and publish Google spreadsheet
1. Go to https://docs.google.com/spreadsheets. Make sure you're logged into your account
2. Create a blank spreadsheet
<img style = "align: center" src="https://github.com/Vintall/Unity-Free-Online-Config/blob/media/Media/Guideline/Guideline1Compressed.png" alt="Guideline1" width="600"/>
3. Go to **File** -> **Share** -> **Publish to web**
<img style = "align: center" src="https://github.com/Vintall/Unity-Free-Online-Config/blob/media/Media/Guideline/Guideline2Compressed.png" alt="Guideline2" width="600"/>
4. In "Publish to the web" pop-up in the left column choose a specific sheet that you want to use as your database. Later on you can make as many as you want.
<img style = "align: center" src="https://github.com/Vintall/Unity-Free-Online-Config/blob/media/Media/Guideline/Guideline3Compressed.png" alt="Guideline3" width="600"/>
5. In the right column pick **Tab-separated values (.tsv)**
<img style = "align: center" src="https://github.com/Vintall/Unity-Free-Online-Config/blob/media/Media/Guideline/Guideline4Compressed.png" alt="Guideline4" width="600"/>
6. In "Published content & settings" you have several fields.
- First of all, choose entire document in first field. 
- You most likely want to have enabled "Automatically republish when changes are made". Otherwise you will need to republish it manually every time. 
- Press **Start publishing** to generate link to your sheet.
<img style = "align: center" src="https://github.com/Vintall/Unity-Free-Online-Config/blob/media/Media/Guideline/Guideline5Compressed.png" alt="Guideline5" width="600"/>
7. Copy the link to your sheet.
<img style = "align: center" src="https://github.com/Vintall/Unity-Free-Online-Config/blob/media/Media/Guideline/Guideline6Compressed.png" alt="Guideline6" width="600"/>

## Connect to spreadsheet in Unity

1. Create two scripts: ExampleDatabase.cs and ExampleVo.cs
2. ExampleVo extend class ASpreadsheetVo. This is one line of data from your database.
```
public class ExampleVo : ASpreadsheetVo
{
    public string ExampleString;
    public int ExampleInt;
    public float ExampleFloat;
    public double ExampleDouble;
}
```
3. ExampleDatabase is a class, that holds database name, url and list of ExampleVo's. Every database have a ScriptableObject, as base class. So, for every database we need to specity attribute [CreateAssetMenu]. You can read more about ScriptableObjects in official unity documentation https://docs.unity3d.com/Manual/class-ScriptableObject.html.
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






