function openTable(objName)
{
	
	if (objName.id == "imgMinus")
	{
		document.all("trTable").style.display = 'none';
		objName.id = "imgPlus";
		objName.src = "<%=this.BP_GifsSrcEnding%>plus.gif";
	}
	else
	{
	document.all("trTable").style.display = 'inline';
	objName.id = "imgMinus";
	objName.src = "<%=this.BP_GifsSrcEnding%>minus.gif";
	}
}

function customWindowOpen(strUrl, strWindow, strParams, iWidth, iHeight)
{
	var iLeft, iTop;
	iLeft = (window.screen.width - iWidth) / 2;
	iTop = (window.screen.height - iHeight) / 2;
	strParams = strParams + ",width=" + iWidth.toString() + ",height=" + iHeight.toString() + ",left=" + iLeft.toString() + ",top=" + iTop.toString();
	window.open(strUrl, strWindow, strParams);
	
}

function ShowHelpPage(strURL)
{
	
	customWindowOpen(strURL,'myWindowOne', 'toolbar=no, menubar=no, location=no, directories=no, scrollbars=yes', 660, 500);
	
}
	
//function changetabmaya (id){
//	switch(id){
//				case 1:
//						document.all["Tab1_Active_Right"].src = "/TASE/Images/Tab_Active_Right.gif";
//						document.all["Tab1_Active_Left"].src = "/TASE/Images/Tab_Active_Left.gif";
//						document.all["Tab1_Active_Text"].style.backgroundImage = 'url(/TASE/Images/Tab_Active_BG.gif)';
//						
//						document.all["Tab2_Active_Right"].src = "/TASE/Images/Tab_Reg_Right.gif";
//						document.all["Tab2_Active_Left"].src = "/TASE/Images/Tab_Reg_Left.gif";
//						document.all["Tab2_Active_Text"].style.backgroundImage = 'url(/TASE/Images/Tab_Reg_BG.gif)';
//						
//						document.all["Tab3_Active_Right"].src = "/TASE/Images/Tab_Reg_Right.gif";
//						document.all["Tab3_Active_Left"].src = "/TASE/Images/Tab_Reg_Left.gif";
//						document.all["Tab3_Active_Text"].style.backgroundImage ='url(/TASE/Images/Tab_Reg_BG.gif)';												
//						
//						document.getElementById("tabCompanies").style.display="block";
//                        try{
//						    document.getElementById("tabCompanies").style.display="table";
//						}
//						catch(e)
//						{
//						    document.getElementById("tabCompanies").style.display="block";
//						}
//						document.getElementById("tabTASE").style.display="none";
//						document.getElementById("tabBoard").style.display="none";
//						
//						
//						break;
//				case 2:
//						document.all["Tab2_Active_Right"].src = "/TASE/Images/Tab_Active_Right.gif";
//						document.all["Tab2_Active_Left"].src = "/TASE/Images/Tab_Active_Left.gif";
//						document.all["Tab2_Active_Text"].style.backgroundImage= 'url(/TASE/Images/Tab_Active_BG.gif)';
//						
//						document.all["Tab1_Active_Right"].src = "/TASE/Images/Tab_Reg_Right.gif";
//						document.all["Tab1_Active_Left"].src = "/TASE/Images/Tab_Reg_Left.gif";
//						document.all["Tab1_Active_Text"].style.backgroundImage = 'url(/TASE/Images/Tab_Reg_BG.gif)';
//						
//						document.all["Tab3_Active_Right"].src = "/TASE/Images/Tab_Reg_Right.gif";
//						document.all["Tab3_Active_Left"].src = "/TASE/Images/Tab_Reg_Left.gif";
//						document.all["Tab3_Active_Text"].style.backgroundImage = 'url(/TASE/Images/Tab_Reg_BG.gif)';												
//						
//						document.getElementById("tabCompanies").style.display="none";
//						try{
//						    document.getElementById("tabTASE").style.display="table";
//						}
//						catch(e)
//						{
//						    document.getElementById("tabTASE").style.display="block";
//						}
//						document.getElementById("tabBoard").style.display="none";
//						
//						break;	
//						
//				case 3:
//						document.all["Tab3_Active_Right"].src = "/TASE/Images/Tab_Active_Right.gif";
//						document.all["Tab3_Active_Left"].src = "/TASE/Images/Tab_Active_Left.gif";
//						document.all["Tab3_Active_Text"].style.backgroundImage= 'url(/TASE/Images/Tab_Active_BG.gif)';
//						
//						document.all["Tab2_Active_Right"].src = "/TASE/Images/Tab_Reg_Right.gif";
//						document.all["Tab2_Active_Left"].src = "/TASE/Images/Tab_Reg_Left.gif";
//						document.all["Tab2_Active_Text"].style.backgroundImage = 'url(/TASE/Images/Tab_Reg_BG.gif)';
//						
//						document.all["Tab1_Active_Right"].src = "/TASE/Images/Tab_Reg_Right.gif";
//						document.all["Tab1_Active_Left"].src = "/TASE/Images/Tab_Reg_Left.gif";
//						document.all["Tab1_Active_Text"].style.backgroundImage = 'url(/TASE/Images/Tab_Reg_BG.gif)';												
//						
//						document.getElementById("tabCompanies").style.display="none";
//						document.getElementById("tabTASE").style.display="none";
//						try{
//						    document.getElementById("tabBoard").style.display="table";
//						}
//						catch(e)
//						{
//						    document.getElementById("tabBoard").style.display="block";
//						}
//						
//						
//						break;						
//	} 
//}

function Request(key)
{

    var strQueryString=QueryString();

    if (strQueryString.length < 1)
			return "";
    arrTmp=strQueryString.split("&");

    for (var i=0; i<arrTmp.length; i++)
    {
		var arrTmp2=arrTmp[i].split("=");
		if (arrTmp2.length < 1)
			continue;

		var curKey=arrTmp2[0];
		var curValue=(arrTmp2.length < 2)?"":arrTmp2[1];
		if (curKey.toLowerCase() == key.toLowerCase())

		return curValue;
    }

    return "";

}
                    
function QueryString()
{
    var strFullUrl=document.location+"";
    var arrTmp=strFullUrl.split("?");
    if (arrTmp.length < 2)
		return "";
    return arrTmp[1];
}
