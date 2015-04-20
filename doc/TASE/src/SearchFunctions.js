var CmbId=0;
var Clicked=0;
function init() {

	document.onkeydown = showKeyDown
}
function showKeyDown(evt) {
	evt = (evt) ? evt : window.event
	if(evt.keyCode==13)
	{
	var pressButton;
	
		if(document.getElementById("sendBtn1")!=null)
			pressButton="";
		else if(document.getElementById("sendBtn")!=null)
			pressButton="";
		else if(document.getElementById("cmdNext")!=null)
			pressButton="cmdNext";
		else if(document.getElementById("cmdLogin")!=null)
			pressButton="cmdLogin";
		else if(document.getElementById("SearchStr1").value!="")
			pressButton="SearchText";
		else if(document.getElementById("btnCalculate") != null)
			pressButton = "btnCalculate";
		else
			pressButton="btnSearchSecurity";
	
		var h = window.document.getElementById(pressButton);
		if(h!=null)
		{
			h.click();
			Clicked =1;		
			h.focus();
		}
		
	}
}
function ShowComboFull(lang)
{

 document.getElementById('cmbSearchTextHeader0text').disabled=true;
var obj=document.getElementById("HeaderUC1_SearchBarUC1_cmbSearchOptions");
var sFileName = "/TASE/SearchJSFiles/BuildCmb" + "_" + obj[obj.selectedIndex].value + "_" + lang + ".js";
LoadScriptFile(sFileName);
setTimeout("ChangeOptionsHP()",700);

}

function ChangeOptionsHP()
{

    var obj=document.getElementById("HeaderUC1_SearchBarUC1_cmbSearchOptions");
    cmbSearchTextHeader0.changeOptionsWithValue(obj[obj.selectedIndex].value);
    

}
function DoSecuritySearchFull(secLink,derLink,message,compLink,etfLink, govBondLink,fundLink)
{
var objCmb=document.getElementById("HeaderUC1_SearchBarUC1_cmbSearchOptions");

var option =objCmb[objCmb.selectedIndex].value;
var TitleHidden="cmbSearchTextHeader0hidden";
var TitleText="cmbSearchTextHeader0text";
var cmbValue=document.getElementById(TitleHidden).value;
var cmbText=document.getElementById(TitleText).value;



if(cmbText!="")
{
var objectType="";
var objectId="";
var securityType="";
var SecTypeCode="";
var EtfTypeCode="";
var shareId="";
var tmp = new Array();

tmp = cmbValue.split('#');

if (tmp.length>=3)
{
	objectId=tmp[0];			
	objectType=tmp[1];
	securityType=tmp[2];

}
var sLink="";
if(option == 7)
{
    if(objectType == "")
    {
        if(SiteLang == 1)
        {
            lang = "eng";
        }
        else
        {
            lang = "heb";
        }
        sLink = "/TASE/Taseredirect.aspx?param="+escape(cmbText)+"&type=sec&res=prd&es=false&lang="+lang;
    }
    else
    {
        sLink = objectType;
    }
}
else if(option==3 && objectId!="")
{
	SecTypeCode=tmp[3];
	EtfTypeCode=tmp[4];
	shareId=tmp[5];
	
	if(SecTypeCode==4 || SecTypeCode==5) 
	{
		sLink=govBondLink;
		sLink=sLink+"?ShareID="+shareId+"&CompanyID="+objectId+"&subDataType=5";
	
	}
	else if(SecTypeCode==6) 
	{
		sLink=etfLink;
		sLink=sLink+"?ShareID="+shareId+"&CompanyID="+objectId+"&subDataType="+EtfTypeCode;
	
	}
	else  
	{
		sLink=compLink;
		sLink=sLink+"?ShareID="+shareId+"&CompanyID="+objectId+"&subDataType="+SecTypeCode;
	}
}
else if(option==6)
{
	sLink=fundLink;
	sLink=sLink+"?objectId="+objectId+"&objectType="+objectType;
}
else if(option!=5)
{
	sLink=secLink;
	sLink=sLink+"?objectId="+objectId+"&objectType="+objectType+"&securityType="+securityType+"&searchTerm="+cmbText;
	
}
else 
{
	sLink=derLink;
	sLink=sLink+"?cmbBase="+cmbValue;
	
}

 window.open(sLink,'_parent');	

		  }
	else
	{
				
		if(Clicked==0)
		{
		alert(message);
		Clicked=0;
				   
		}
	}		
}


function DoSearchFull(searchValue,link,message)
{
	
   if(searchValue!="")
   {
 
          var theform = document.forms[0];
          theform.action=link;
          theform.action=theform.action+searchValue;
          try{
          document.getElementById('__VIEWSTATE').outerHTML="";
		  document.getElementById('__EVENTARGUMENT').outerHTML="";
		  document.getElementById('__EVENTTARGET').outerHTML="";	
		  } catch(e){}	
		  theform.submit();	  	
		 
   }
   else
   {
      alert(message);
   }
}

function LoadScriptFile(jsFile)
{
    var script = document.createElement('script'); 
    script.type = 'text/javascript'; 
    script.src = jsFile; 
    document.getElementsByTagName('head')[0].appendChild(script);  
}
