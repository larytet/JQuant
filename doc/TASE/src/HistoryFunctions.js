function changeRadio()
{
    ChangeFrequency('8', true);
    document.getElementById("HistoryData1_rbPeriod8").checked = "checked";
}
function changeExtraRadio(id)
{
    try
    {
	    if(id==1)
	    {
		    document.getElementById("HistoryData1_RBExtraPeriodList_4").checked = "checked";
		    document.getElementById("HistoryData1_RBrangeAfter").checked = false;
		    document.getElementById("HistoryData1_txtTradeDaysAfter").innerText="";
		    document.getElementById("HistoryData1_RBrangeBefor").checked = false;
		    document.getElementById("HistoryData1_txtTradeDaysBefor").innerText="";
    		
	    }
	    if(id==2)
		    document.getElementById("HistoryData1_RBrangeAfter").checked = "checked";
	    if(id==3)
		    document.getElementById("HistoryData1_RBrangeBefor").checked = "checked";
    	
	    if(id==2 || id==3)
	    {
		    document.getElementById("HistoryData1_RBExtraPeriodList_0").checked = false;
		    document.getElementById("HistoryData1_RBExtraPeriodList_1").checked = false;
		    document.getElementById("HistoryData1_RBExtraPeriodList_2").checked = false;
		    document.getElementById("HistoryData1_RBExtraPeriodList_3").checked = false;
		    document.getElementById("HistoryData1_RBExtraPeriodList_4").checked = false;
	    }
	}
	catch(e){}
		
}
function ClearNoData()
{
	try
	{	    
		document.getElementById("trResult").style.display="none";
		if (document.getElementById("HistoryData1_labelNoData"))
		{		
		document.getElementById("HistoryData1_labelNoData").innerText="" ;
		}
			
	}
	catch(e)
	{
	
	}
}
var ExtraId=2;
function ChangeAdditionalQueries(id)
{
    
	ClearNoData();
	ExtraId=id;
	
	try
	{
	if(document.getElementsByName("HistoryData1$RBExtraTypeList")[id].checked==true)
	{
	
		if(document.getElementsByName("HistoryData1$RBExtraPeriodList")[0].checked==true || document.getElementsByName("HistoryData1$RBExtraPeriodList")[1].checked==true)
		{   
		    	
		        document.getElementById("trExtraFrequency").style.display="inline";
				document.getElementById("tdExtraMonth").style.display="inline";
				
				
				document.getElementById("tdExtraYear").style.display="none";
				document.getElementsByName("HistoryData1$rbExtraFrequency")[0].checked=true;
				
				document.getElementsByName("HistoryData1$rbExtraFrequency")[1].checked=false;
		}
		else
		{
			    document.getElementById("trExtraFrequency").style.display="inline";
				document.getElementById("tdExtraMonth").style.display="inline";
				document.getElementById("tdExtraYear").style.display="inline";
				if(document.getElementsByName("HistoryData1$rbExtraFrequency")[1].checked==true)
				{
					document.getElementsByName("HistoryData1$rbExtraFrequency")[0].checked=false;
					document.getElementsByName("HistoryData1$rbExtraFrequency")[1].checked=true;
				}
				else
				{
					document.getElementsByName("HistoryData1$rbExtraFrequency")[0].checked=true;
					document.getElementsByName("HistoryData1$rbExtraFrequency")[1].checked=false;
				}
		}
		
	}
	else
	{
	   document.getElementById("trExtraFrequency").style.display="none";
	   document.getElementById("tdExtraMonth").style.display="none";
	   document.getElementById("tdExtraYear").style.display="none";
	}
	}
	catch(e){}
}
function ChangeAdditionalStartUp(id)
{

	
	try
	{
	if(document.getElementsByName("HistoryData1$RBExtraTypeList")[id].checked==true || id==1)
	{
	
		if(document.getElementsByName("HistoryData1$RBExtraPeriodList")[0].checked==true || document.getElementsByName("HistoryData1$RBExtraPeriodList")[1].checked==true)
		{
		
		         document.getElementById("trExtraFrequency").style.display="inline";
				document.getElementById("tdExtraMonth").style.display="inline";
				document.getElementsByName("HistoryData1$rbExtraFrequency")[0].checked=true;
				document.getElementById("tdExtraYear").style.display="none";
				document.getElementsByName("HistoryData1$rbExtraFrequency")[1].checked=false;
		}
		else
		{
			   document.getElementById("trExtraFrequency").style.display="inline";
				document.getElementById("tdExtraMonth").style.display="inline";
				document.getElementById("tdExtraYear").style.display="inline";
				if(document.getElementsByName("HistoryData1$rbExtraFrequency")[1].checked==true)
				{
					document.getElementsByName("HistoryData1$rbExtraFrequency")[0].checked=false;
					document.getElementsByName("HistoryData1$rbExtraFrequency")[1].checked=true;
				}
				else
				{
					document.getElementsByName("HistoryData1$rbExtraFrequency")[0].checked=true;
					document.getElementsByName("HistoryData1$rbExtraFrequency")[1].checked=false;
				}
		}
	}
	else
	{
	   document.getElementById("trExtraFrequency").style.display="none";
	   document.getElementById("tdExtraMonth").style.display="none";
	   document.getElementById("tdExtraYear").style.display="none";
	}
	}
	catch(e){}
}
function ChangeAdditionalQueriesRangeCalendar(id)
{
		
	ClearNoData();
	try
	{
	document.getElementsByName("HistoryData1$RBExtraPeriodList")[0].checked=false;
	document.getElementsByName("HistoryData1$RBExtraPeriodList")[1].checked=false;
	document.getElementsByName("HistoryData1$RBExtraPeriodList")[2].checked=false;
	document.getElementsByName("HistoryData1$RBExtraPeriodList")[3].checked=false;
	document.getElementsByName("HistoryData1$RBExtraPeriodList")[4].checked=false;
	if(document.getElementsByName("HistoryData1$RBExtraTypeList")[id].checked==true)
	{
	    document.getElementById("trExtraFrequency").style.display="inline";
		document.all.tdExtraMonth.style.display="inline";
		document.getElementsByName("HistoryData1$rbExtraFrequency")[0].checked=true;
		document.all.tdExtraYear.style.display="inline";
		document.getElementsByName("HistoryData1$rbExtraFrequency")[1].checked=false;
	}
	else
	{
	     document.getElementById("trExtraFrequency").style.display="none";
		document.all.tdExtraMonth.style.display="none";
		document.all.tdExtraYear.style.display="none";
	}
	}
	catch(e){}
				
}
function ChangeAdditionalQueriesRange(id)
{
	
	ClearNoData();
	//if (id=1)
	//    id=0;
	try
	{			
	if (document.getElementsByName("HistoryData1$RBrange").length > 0)
	{
	    document.getElementsByName("HistoryData1$RBrange")[0].checked=false;
	    document.getElementsByName("HistoryData1$RBrange")[1].checked=false;
	    document.getElementById("HistoryData1_txtTradeDaysAfter").innerText="";
	    document.getElementById("HistoryData1_txtTradeDaysBefor").innerText="";
	}
	if(document.getElementsByName("HistoryData1$RBExtraTypeList")[id].checked==true)
	{
			
		if(document.getElementsByName("HistoryData1$RBExtraPeriodList")[0].checked==true || document.getElementsByName("HistoryData1$RBExtraPeriodList")[1].checked==true)
		{
		         document.getElementById("trExtraFrequency").style.display="inline";
				document.all.tdExtraMonth.style.display="inline";
				document.getElementsByName("HistoryData1$rbExtraFrequency")[0].checked=true;
				document.all.tdExtraYear.style.display="none";
				document.getElementsByName("HistoryData1$rbExtraFrequency")[1].checked=false;
		}
		else
		{
			     document.getElementById("trExtraFrequency").style.display="inline";
				document.all.tdExtraMonth.style.display="inline";
				document.getElementsByName("HistoryData1$rbExtraFrequency")[0].checked=true;
				document.all.tdExtraYear.style.display="inline";
				document.getElementsByName("HistoryData1$rbExtraFrequency")[1].checked=false;
		}
	}
	else
	{
	   document.getElementById("trExtraFrequency").style.display="none";
	   document.all.tdExtraMonth.style.display="none";
	   document.all.tdExtraYear.style.display="none";
	}
	}
	catch(e){}
}
function ChangeFrequency(id, changeFrequency)
{
	try
	{
	
	if(id==-1)
	{
		for(i=1;i<8;i++)
		{
			if(document.getElementById("HistoryData1_rbPeriod"+i).checked)	
			{
				id=i;
			}
		}
	}
	
	if(id==1)
	{
	   document.all.tdDay.style.display="inline";
	   document.all.tdWeek.style.display="none";
	   document.all.tdMonth.style.display="none";
	   document.all.tdYear.style.display="none";
	}
	else if(id==2)
	{
	   document.all.tdDay.style.display="inline";
	   document.all.tdWeek.style.display="inline";
	   document.all.tdMonth.style.display="inline";
	   document.all.tdYear.style.display="none";
	}
	else if(id==3)
	{
	   document.all.tdDay.style.display="inline";
	   document.all.tdWeek.style.display="inline";
	   document.all.tdMonth.style.display="inline";
	   document.all.tdYear.style.display="none";
	}
	else if(id==4)
	{
	   document.all.tdDay.style.display="inline";
	   document.all.tdWeek.style.display="inline";
	   document.all.tdMonth.style.display="inline";
	   document.all.tdYear.style.display="none";
	}
	else if(id==5)
	{
	   document.all.tdDay.style.display="inline";
	   document.all.tdWeek.style.display="inline";
	   document.all.tdMonth.style.display="inline";
	   document.all.tdYear.style.display="none";
	}
	else if(id==6)
	{
	   document.all.tdDay.style.display="inline";
	   document.all.tdWeek.style.display="inline";
	   document.all.tdMonth.style.display="inline";
	   document.all.tdYear.style.display="inline";
	}
	else if(id==7)
	{
	   document.all.tdDay.style.display="inline";
	   document.all.tdWeek.style.display="inline";
	   document.all.tdMonth.style.display="inline";
	   document.all.tdYear.style.display="inline";
	}
	else if(id==8)
	{
	
	   document.all.tdDay.style.display="inline";
	   document.all.tdWeek.style.display="inline";
	   document.all.tdMonth.style.display="inline";
	   document.all.tdYear.style.display="inline";
	}	
	else if(id==9)
	{
	
	   document.all.tdDay.style.display="inline";
	   document.all.tdWeek.style.display="inline";
	   document.all.tdMonth.style.display="inline";
	   document.all.tdYear.style.display="inline";
	}	
	
	if(changeFrequency)
	   document.getElementsByName("HistoryData1$rbFrequency")[0].checked=true;
	   }
	   catch(e){}
}