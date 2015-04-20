var time;
var xposition=0,yposition=0;
var Was_Clicked=false;
var idivxHeb=0;
var intSecurityType=0;


function cmb(name,form,width,height,locked,sorted,valueName,defaultText,defaultValue,direction,showCmbButton,divXHeb,SecurityType, OnChangeFunction)
{

    //debugger
	if (width=="" || width==null) this.width=20;
		 else this.width=width;
	if (height=="" || height==null) this.height=5;
		else this.height=height;
	if (locked==true) this.locked=true;
		else this.locked=false;
	if (sorted==true) this.sorted=true;
		else this.sorted=true;
	if (valueName=="" || valueName==null) this.valueName=name+"hidden";
		 else this.valueName=valueName;
	if (defaultText==null) this.defaultText="";
		else this.defaultText=defaultText;
	if (defaultValue==null) this.defaultValue="";
		else this.defaultValue=defaultValue;
	if (OnChangeFunction==null) this.OnChangeFunction="";
		else {this.OnChangeFunction=OnChangeFunction;}
		
			
	this.status=0;
	this.flag=0;
	this.flagFocus=0;
	this.lastText="";
	this.maxText="";
	this.dir=direction;
	if(showCmbButton==null) showCmbButton=true;
	this.showCmbButton=showCmbButton;
	
			
	this.name=name;
	this.type='cmb';
	
	this.options=new Array();
	this.options[0]=new Array();
	this.options[1]=new Array();
	this.strOptions="";
	
	this.form=form;
	this.hidden=null;
	this.text=null;
	this.div=null;
	this.select=null;

	this.getText=getText;
	this.getValue=getValue;
	
	this.setText=setText;
	this.setValue=setValue;
		
	this.addOption=addOption;
	this.addOptions=addOptions;
	this.changeOptions=changeOptions;
	this.changeOptionsWithValue = changeOptionsWithValue;
	this.create=create;
	this.divXHeb=divXHeb;
	if(divXHeb==null) idivXHeb=0;
	else idivXHeb=divXHeb;
	
	
	if(SecurityType!=null)  intSecurityType=SecurityType;
	else intSecurityType=0;
	
	
}

function create(disableCntr)
{

if(this.showCmbButton==true)
	document.write (editFontStart+'<table cellspacing=0 cellpadding=0 border=0 >'
	    +'<tr><td><INPUT class="ComboTextBox" type="text" '
	    +'dir="' + this.dir + '" id="' + this.name + 'text" '
	    +'name="' + this.name + 'text" size="'+ this.width +'" '
	    +'onkeyup="IEevent('+this.name+',event)" onclick="IEevent('+this.name+',event)" '
	    +'onblur=textBlur('+this.name+') onfocus="'+this.name+'.flag=1;" '
	    +'value="'+ this.defaultText +'" '  + disableCntr +' '
	    +'autoComplete=off style="height:20px"></td><td valign=top align=right >'
	    +'<img border="0" id="btn' + this.valueName + '" name="btn' + this.valueName + '" '
	    +'src="/TASE/Images/btnCombo.jpg" onclick="javascript:FocusText('+this.name+',event);"></td></tr>'
	    +'<tr><td colspan=2><div id="' + this.name + 'flyoutReferenceContainer" style="position:relative;font-size:0;height:0;"></div></td></tr>'
	    +'</table>'+editFontEnd);
else
	document.write (editFontStart+'<table cellspacing=0 cellpadding=0 border=0 ><tr><td>'
	    +'<INPUT class="TextBox" type="text" dir="' + this.dir + '" id="' + this.name + 'text" '
	    +'name="' + this.name + 'text" size="'+ this.width +'" '
	    +'onkeyup="IEevent('+this.name+',event)" onclick="IEevent('+this.name+',event)" '
	    +'onblur=textBlur('+this.name+') onfocus="'+this.name+'.flag=1;" '
	    +'value="'+ this.defaultText +'" '  + disableCntr +' autoComplete=off><div id="' + this.name + 'flyoutReferenceContainer" style="position:relative;font-size:0;height:0;z-index=999"></div></td></tr></table>'+editFontEnd);



	document.write ('<INPUT type="hidden" id="' + this.valueName + '" class="TextBox" name="' + this.valueName + '" value="'+ this.defaultValue +'" size="'+ this.width +'">');
	this.text=eval("document."+this.form+"."+ this.name + "text");   
	
	this.hidden=eval("document."+this.form+"."+ this.valueName);   



	if (this.sorted==false)
	{
		for(i=0;i<this.options[0].length-1;i++)
			for(j=i;j<this.options[0].length;j++)
				if(this.options[0][j].toUpperCase()<this.options[0][i].toUpperCase())
				{
					tmp=this.options[0][j];
					this.options[0][j]=this.options[0][i];
					this.options[0][i]=tmp;
					
					tmp=this.options[1][j];
					this.options[1][j]=this.options[1][i];
					this.options[1][i]=tmp;
				}
	}
	

	for(i=1;i<this.options[0].length-1;i++)
		if(this.options[0][i].length>this.maxText.length)
			this.maxText=this.options[0][i];

	for(i=0;i<this.options[0].length;i++)
		this.strOptions += "<option value='" + this.options[1][i] + "'>" + this.options[0][i]+"</option>";

	this.status=1;
	this.text.defaultChecked=true;
}

function addOption(text,value)
{
	if (value==null) value="";
	if (text==null) value="";
	this.options[0][this.options[0].length]=text;
	this.options[1][this.options[1].length]=value;
}

function addOptions(strText,strValue,arrValue,arrText) {
if (strText==null) strText=""; 
 if(strValue==null) strValue="";
 
 
	this.options[0]=arrText.split(",");
	this.options[1]=arrValue.split(",",arrText.length); 
} 

function changeOptions(sugComp,lang )
{
	intSecurityType=sugComp;
    var strText,strValue;
  try
  {
  strText=getT(sugComp,lang); 
  strValue=getV(sugComp,lang);  
  }
  catch(err)
  {
  return;
  }
  this.options[0]=strText.split(",");
  this.options[1]=strValue.split(",",strText.length); 
 
  this.strOptions="";
  

	for(i=1;i<this.options[0].length-1;i++)
		if(this.options[0][i].length>this.maxText.length)
			this.maxText=this.options[0][i];


	for(i=0;i<this.options[0].length;i++)
		this.strOptions += "<option value='" + this.options[1][i] + "'>" + this.options[0][i];
	
	this.status=1;
	this.text.defaultChecked=true;
	this.setText();
	this.setValue();	
	
	this.flagFocus=1;

	this.text.focus();
	this.flagFocus=0;	
}

function changeOptionsWithValue(SecurityType)
{

    intSecurityType = SecurityType;
    var strText,strValue;

    try
    {
        strText=getT();
        strValue=getV();
    }
    catch(err)
      {
      return;
      }
    this.options[0]=strText.split(",");
    this.options[1]=strValue.split(",",strText.length); 

    this.strOptions="";


    for(i=1;i<this.options[0].length-1;i++)
        if(this.options[0][i].length>this.maxText.length)
            this.maxText=this.options[0][i];


    for(i=0;i<this.options[0].length;i++)
        this.strOptions += "<option value='" + this.options[1][i] + "'>" + this.options[0][i];

    this.status=1;
    this.text.defaultChecked=true;
    this.setText();
    this.setValue();	

  this.text.disabled=false;
    this.flagFocus=1;
    this.text.focus();
    this.flagFocus=0;
    
    	
}


function setText()
{

	if (this.status>0)
		this.text.value="";


}

function setValue()
{

	if (this.status>0)
		this.hidden.value="";

}

function getText()
{

	if (this.status>0)
		return this.text.value;
}

function getValue()
{

	if (this.status>0)
		return this.hidden.value;
}

function IEevent(obj,e)
{
	if (obj.status==1)
		makeDiv(obj.name);

	getCords(obj);
		
	if (e.keyCode==13)
	{

		if (obj.div.style.visibility=='visible' && obj.select.selectedIndex>=0)
		{
			obj.text.value=obj.select.options[obj.select.selectedIndex].text;
			
			obj.div.style.visibility='hidden';
		}
		else if (obj.form=='Form1') 
		{ 
			search();
		}
	}
	else if(e.keyCode!=9)
		 {
			if(e.keyCode==0 || e.which && e.which == 1)
				obj.text.value="";
			openDiv(obj,xposition,yposition+20);
			if(e.keyCode==40)
			{
				obj.text.blur();
				obj.select.focus();
				obj.flag=1;
			}
			
		 }
		
		obj.hidden.value=obj.select.options[obj.select.selectedIndex].value;
		

}


function openDiv(obj,x,y)
{
	obj.flag=1;
	div=eval("obj.div"+sty);
	div.visibility='visible';
	
	fillSelect(obj,x);
	
	obj.select.selectedIndex=0;

		
}

function fillSelect(obj,x)
{

	text = obj.text.value.replace(/([\(\)\*\.])/, "\\$1");
	
	try
	{
	regExp = new RegExp('(<option value=[^>]+>'+ text +'[^<]*)',"gi");
	
	if (regExp.test(obj.strOptions))
	{
		
		arrSelect=obj.strOptions.match(regExp);
		strSelect=arrSelect.join("");
		if (arrSelect.length<obj.height)
			size=arrSelect.length;
		else
			size=obj.height;
			
		if (size<2)
			size=2;
	}
	else
	{
		size=2;
		strSelect="<option value='-------'>-------<option>";
	}
	}catch(e){}
		
	obj.div.innerHTML = '<select  id="' + obj.name + 'select" dir=' + obj.dir + ' name="' + obj.name + 'select" '
	    +'onfocus="'+ obj.name +'.flag=1;choose(this,'+obj.name+')" '
	    +'onchange="choose(this,'+obj.name+');'+obj.OnChangeFunction+'" '
	    +'onblur="textBlur('+obj.name+');" onkeydown=selectup('+obj.name+',event) '
	    +'onclick="selectup('+obj.name+',event)" size='+ size + ' '
	    +'style="z-index:1000;background-color:white">' + strSelect + "</select>";

    if (navigator.userAgent.indexOf("Firefox")!=-1 || navigator.userAgent.indexOf("Safari")!=-1)
	{
	    obj.select = document.getElementById(obj.name+'select');
	}
	else
	{
	    //IE
	    eval(obj.name+'.select ='+obj.name+'div.all.'+obj.name+'select');
	}
    setTimeout('',100);
        if(obj.select.offsetWidth < obj.text.offsetWidth) 
        {
        
            if(obj.showCmbButton==true)
            {
                obj.select.style.width = parseInt(obj.text.offsetWidth+20) + "px";
            }
            else
            {
                obj.select.style.width = obj.text.offsetWidth + "px";
            }
    }
    
}

function getCords(obj)
{
	var curObject=obj.text;

	xposition=0;
	yposition=0; 
	 
	do
	{
		xposition += curObject.offsetLeft;
		yposition += curObject.offsetTop;
		curObject=curObject.offsetParent;
		
	}while(curObject.tagName!='BODY');
	
	if (obj.dir.toLowerCase() == "rtl")
	{
		xposition -=eval(idivXHeb);
	}
}

function makeDiv(objName)
{
     objRef = eval(objName);
     
     var newDiv = document.createElement("DIV");
     
     newDiv.id = objName + "div";   
     newDiv.name= objName + "div";
     newDiv.align= "right"   
     newDiv.style.position = "absolute";
     newDiv.style.top = "0";
     newDiv.style.visibility = "hidden";
     newDiv.style.backgroundColor="white";
     newDiv.style.zIndex='1000';
     
     if (objRef.dir.toLowerCase() == "rtl")
        newDiv.style.right = "0";
     else
        newDiv.style.left = "0";     
        
    var elemDiv = document.getElementById(objName + "div");

    if(elemDiv!=null)
    {
        elemDiv.parentNode.removeChild(elemDiv);
    }
     
     window[objName + "div"] = objRef.div = document.getElementById(objName + "flyoutReferenceContainer").appendChild(newDiv);
     
     objRef.status = 2;  
}

function textBlur(obj)
{
	obj.flag=0;
	time=setTimeout('divoff('+obj.name+')',50);
}

function selectup(obj,e)
{

    if(e.keyCode==38)
    {
		if (obj.select.selectedIndex==0)
		{
			obj.select.selectedIndex=-1;
			obj.text.focus();
		}
	}
	else if (e.keyCode==13 || e.type=='click')  
	{
		
		obj.div.style.visibility='hidden';
		try
		{
		  if ((e.keyCode==13) && (obj.form=='Form1')) 
		  {
		  search();
		  }
		}
		catch(e){}
	}
}

function divoff(obj)
{
	flag=0;
	if (obj.flag=='0' && obj.div && document.activeElement!=obj.select && obj.text.value!="")
	{
		
		for(i=0;i<obj.options[0].length && flag!=1;i++)
		{
			
			if(obj.options[0][i]==obj.text.value && obj.hidden.value!='-------')
			{
				flag=1;
				if (obj.options[1][i])
					obj.hidden.value=obj.options[1][i];
				else
					obj.hidden.value=obj.text.value;
			}
			else if (obj.hidden.value=='-------')
			{
				
			}
			else
			{
				obj.text.value=obj.select.options[obj.select.selectedIndex].text;
				obj.hidden.value=obj.select.options[obj.select.selectedIndex].value;
					
				
			}
		}

		if (obj.locked && flag==0)
		{
			alert("יש לבחור רק מהרשימה");
			
		}
		else
			eval("obj.div"+sty).visibility='hidden';
	}
	else if (obj.text.value=="")
		obj.hidden.value="";
		
	if (obj.flag=='0' && obj.div && obj.text.value=="")
		eval("obj.div"+sty).visibility='hidden';
	eval(obj.OnChangeFunction);
}

function choose(select,obj)
{
	if (obj.flagFocus==0)
	{
	    obj.text.value=select.options[select.selectedIndex].text;
    	obj.hidden.value=select.options[select.selectedIndex].value;
    
	}
	
}

function FocusText(obj,e)
{		
	obj.setText();
	obj.setValue();	
	IEevent(obj,e);
	obj.flag=1;
	try
	{
	obj.text.focus();
	}
	catch(err)
      {
      return;
      }

}

doc="document.all.";
sty=".style";
editFontStart='<font class="selectCompanyGroup">';
editFontEnd='</font>';
