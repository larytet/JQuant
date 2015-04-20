

function callback_OpenSecondLevel(res)
{
	
	if(res.value!= null && res.value!="" )
	{
		arrRes=new Array();
		arrRes=res.value.split("^");
		
		document.getElementById(arrRes[0]).style.display = "inline";
		document.getElementById(arrRes[0]).innerHTML="<table cellSpacing='0' cellPadding='0' border='0'>"+arrRes[1]+"</table>";
	
		

	}
}
function OpenSecondLevel(objId, channelPath,  RootChannelName, CurrentChannel, CMSMode)
{

	
	if (document.getElementById(objId).style.display =="inline") //close current menu
	{
		document.getElementById(objId).style.display = "none";
		document.getElementById(objId).innerHTML="";
		
	}
	else
	{
		OpenCloseElement(objId,"span","blue","openOrig1",RootChannelName);
		AjaxMenuUC.OpenSecondLevel(channelPath, objId, RootChannelName,CurrentChannel, CMSMode, callback_OpenSecondLevel);
	}
	
}

function callback_OpenThirdLevel(res)
{
	
	if(res.value!= null && res.value!="" )
	{
	
		arrRes=new Array();
		arrRes=res.value.split("^");
		strObj=arrRes[0];
		document.getElementById(arrRes[0]).style.display = "inline";
		document.getElementById("imgtr2"+strObj.replace("span2","")).src="/TASE/Images/menu_orange_open.gif";
		//document.getElementById("font2"+strObj.replace("span2","")).className="divOut2Active";
		//document.getElementById("font2"+strObj.replace("span2","")).onmouseover="this.className=DivOver2Active";
		//document.getElementById("font2"+strObj.replace("span2","")).onmouseout="this.className=divOut2Active";
		document.getElementById(arrRes[0]).innerHTML="<table cellSpacing='0' cellPadding='0' border='0'>"+arrRes[1]+"</table>";
	
		

	}
}
function OpenThirdLevel(objId, channelPath,  RootChannelName, CurrentChannel, spanName, CMSMode)
{

	//alert(objId);
	if (document.getElementById(objId).style.display =="inline")
	{
		
		document.getElementById(objId).style.display = "none";
		document.getElementById(objId).innerHTML="";
	}
	else
	{
	
		OpenCloseElement(objId,spanName,"orange", "openOrig2", RootChannelName);
		AjaxMenuUC.OpenThirdLevel(channelPath, objId, RootChannelName,CurrentChannel, CMSMode, callback_OpenThirdLevel);
		
		
	}
	
}

function OpenCloseElement(objId, spanName, colorName, openOrigName, RootChannelName)
{
	if(document.getElementById)
	{
		var el = document.getElementById(objId);
		var ar = document.getElementById("masterdiv").getElementsByTagName("span");
		
		var arIm = document.getElementById("masterdiv").getElementsByTagName("img"); //DynamicDrive.com change	
		
		if(RootChannelName=="TASEEng")
		{
			imgFolder="English";
		}
		else
		{
			imgFolder="Hebrew";
		}
		
		if(el.style.display != "inline")
		{
			for (var i=0; i<ar.length; i++)
			{
			    if (ar[i].id.indexOf(spanName)!=-1) 
			    {
					ar[i].innerHtml="";
					ar[i].style.display = "none";
				 }
				
			}		
			
			for (var i=0; i<arIm.length; i++)
			{	
						
				if (arIm[i].id.indexOf(objId.replace(spanName,""))==-1)
				{
				  if(arIm[i].id.indexOf("imgtr2")!=-1)
						arIm[i].src = "/TASE/Images/"+imgFolder+"/menu_"+colorName+"_closed.gif";
				}				
			}	
						
			el.style.display = "inline";
			
						
			
			
		}
		else
		{
			el.innerHtml="";
			el.style.display = "none";
			
		}
	}
}

function SwitchMenuFull(obj, divName, colorName, openOrigName, gifending)
{
	if(document.getElementById)
	{
		var el = document.getElementById(obj);
		var ar = document.getElementById("masterdiv").getElementsByTagName("span"); 
		var arIm = document.getElementById("masterdiv").getElementsByTagName("img");
		var im = document.getElementById("img"+obj);
		
		if(el.style.display != "inline")
		{ 
		
			for (var i=0; i<ar.length; i++)
			{
			    if (ar[i].id.indexOf(divName)!=-1) 
				  ar[i].style.display = "none";
				
			}		
			
			for (var i=0; i<arIm.length; i++)
			{				
				if (arIm[i].id.indexOf(divName)!=-1) 
				{				 
				  if(eval("arIm[i]."+openOrigName) == "0")
				    arIm[i].src = gifending+"menu_"+colorName+"_closed.gif";
				}				
			}	
						
			el.style.display = "inline";
			im.src="/TASE/Images/menu_"+colorName+"_open.gif";
		}
		else
		{
			el.style.display = "none";
			
			if(eval("im."+openOrigName) == "0")
			  im.src=gifending+"menu_"+colorName+"_closed.gif";  
		}
	}
}



