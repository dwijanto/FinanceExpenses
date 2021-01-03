Imports Microsoft.Exchange.WebServices.Data

Imports System.IO
Imports System.Threading
Imports System.Text

Public Class FormGetEmail
    Dim service As ExchangeService
    Dim myModel As EmailModel
    Dim myThreadDelegate As New ThreadStart(AddressOf DoWork)
    Dim myThread As New System.Threading.Thread(myThreadDelegate)
    Dim AutoTask As Boolean = True
    Delegate Sub ProgressReportDelegate(ByVal id As Integer, ByRef message As String)
    Dim url As String = "https://mail-eu.seb.com/ews/exchange.asmx"
    Dim username As String = "sebshipdoc"
    Dim password As String = "honscH@ndfrgz01"
    Dim mybasefolder As String = "c:\temp\documents"
    Dim mailbox As String = String.Empty
    Dim bs As BindingSource
    Dim bs2 As BindingSource
    'Dim cm As CurrencyManager

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        If Not myThread.IsAlive Then
            Me.ToolStripStatusLabel1.Text = ""
            Me.ToolStripStatusLabel2.Text = ""

            'Get file
            AutoTask = False
            myThread = New Thread(AddressOf DoWork)
            myThread.Start()
        Else
            MessageBox.Show("Process still running. Please Wait!")
        End If
    End Sub

    Sub DoWork()

        If checkLockFile(Application.StartupPath & "\log\GetAttachment.lck") Then
            If Not AutoTask Then
                If Not MessageBox.Show("Process is running in different computer! Force to continue? ", "Question", MessageBoxButtons.OKCancel) = DialogResult.OK Then
                    Exit Sub
                End If
            End If
        End If
        ProgressReport(6, "Marque")
        If GetFolder01(0) Then
            ProgressReport(2, "Done")
            ProgressReport(1, "")
        End If
        ProgressReport(5, "Continuous")
        File.Delete(Application.StartupPath & "\log\GetAttachment.lck")
    End Sub
    Private Sub ProgressReport(ByVal id As Integer, ByRef message As String)
        If Me.InvokeRequired Then
            Dim d As New ProgressReportDelegate(AddressOf ProgressReport)
            Me.Invoke(d, New Object() {id, message})
        Else
            Select Case id
                Case 1
                    Me.ToolStripStatusLabel1.Text = message
                Case 2
                    Me.ToolStripStatusLabel2.Text = message
                Case 4
                    ' Me.Label4.Text = message
                Case 5
                    ToolStripProgressBar1.Style = ProgressBarStyle.Continuous
                Case 6
                    ToolStripProgressBar1.Style = ProgressBarStyle.Marquee
                Case 7
                    Dim myvalue = message.ToString.Split(",")
                    ToolStripProgressBar1.Minimum = 1
                    ToolStripProgressBar1.Value = myvalue(0)
                    ToolStripProgressBar1.Maximum = myvalue(1)
            End Select

        End If

    End Sub

    Private Function GetFolder01(ByVal offset As Integer) As Boolean
        'ProgressReport(1, "Get Folder")
        'Dim mydoctype As Integer

        Dim savingfolder As String = String.Empty
        Dim myfilenamelog As String = String.Empty
        Dim myreturn As Boolean = False
        Dim sqlstr = "select dt.* from ssc.paramdt dt" &
             " left join ssc.paramhd hd on hd.paramhdid = dt.paramhdid" &
             " where hd.paramname= 'ssc'" &
             " order by dt.ivalue;" &
            " select * from ssc.sscemailhd;" &
            " select * from ssc.sscemaildt;"

        Dim mymessage As String = String.Empty

        Dim ds As New DataSet
        ds = DataAccess.GetDataSet(sqlstr, CommandType.Text)
        Dim mylastdate As DateTime = ds.Tables(0).Rows(0).Item("ts")
        url = ds.Tables(0).Rows(1).Item("cvalue")
        username = ds.Tables(0).Rows(2).Item("cvalue")
        password = ds.Tables(0).Rows(3).Item("cvalue")
        mybasefolder = ds.Tables(0).Rows(4).Item("cvalue")
        mailbox = ds.Tables(0).Rows(5).Item("cvalue")
        Dim mylastdateinvoice As DateTime = ds.Tables(0).Rows(0).Item("ts")
        Dim mylastdatepackinglist As DateTime = ds.Tables(0).Rows(0).Item("ts")

        Try
            'ProgressReport(1, "After Get DataSet")
            'Header and Detail
            ds.Tables(0).TableName = "PARAM"

            ds.Tables(1).TableName = "SSCEmailHD"
            ds.Tables(1).CaseSensitive = True
            ds.Tables(2).TableName = "SSCEmailDT"
            ds.Tables(2).CaseSensitive = True

            Dim idx1(2) As DataColumn
            idx1(0) = ds.Tables(1).Columns("emailsubject")
            idx1(1) = ds.Tables(1).Columns("receiveddate")
            idx1(2) = ds.Tables(1).Columns("sender")
            ds.Tables(1).PrimaryKey = idx1
            ds.Tables(1).Columns(0).AutoIncrement = True
            ds.Tables(1).Columns(0).AutoIncrementSeed = -1
            ds.Tables(1).Columns(0).AutoIncrementStep = -1
            
            Dim idx2(1) As DataColumn
            idx2(0) = ds.Tables(2).Columns("hdid")
            idx2(1) = ds.Tables(2).Columns("attachmentname")
            ds.Tables(2).PrimaryKey = idx2
            ds.Tables(2).Columns(0).AutoIncrement = True
            ds.Tables(2).Columns(0).AutoIncrementSeed = -1
            ds.Tables(2).Columns(0).AutoIncrementStep = -1
            

            Dim rel As DataRelation
            Dim hcol As DataColumn
            Dim dcol As DataColumn

            hcol = ds.Tables(1).Columns("id") 'docemailhdid in table header
            dcol = ds.Tables(2).Columns("hdid") 'docemailhdid in table dtl
            rel = New DataRelation("hdrel", hcol, dcol)
            ds.Relations.Add(rel)

            bs = New BindingSource
            bs2 = New BindingSource
            bs.DataSource = ds.Tables(1)
            bs2.DataSource = bs
            bs2.DataMember = "hdrel"
            'cm = CType(Me.BindingContext(ds.Tables(1)), CurrencyManager)
            'cm.Position = 0
        Catch ex As Exception
            Logg(ex.Message)
            ProgressReport(1, ex.Message)
            Return myreturn
        End Try

        Dim totalview As Integer = Integer.MaxValue

        Using myservice As New ClassEWS(url, username, password, "as", False)
            service = myservice.CreateConnection()

            Dim view As FolderView = New FolderView(totalview)
            view.PropertySet = New PropertySet(BasePropertySet.IdOnly)
            view.PropertySet.Add(FolderSchema.DisplayName)
            view.Offset = offset
            'MessageBox.Show(view.Offset)
            Dim searchFilter As SearchFilter = New SearchFilter.IsGreaterThan(FolderSchema.TotalCount, 0)
            view.Traversal = FolderTraversal.Deep
            Try
                
                Dim userMailbox = New Mailbox(mailbox)
                Dim folderId = New FolderId(WellKnownFolderName.Root, userMailbox)
                Dim results As FindFoldersResults = service.FindFolders(folderId, searchFilter, view)

                Dim searchFilterCollection As List(Of SearchFilter) = New List(Of SearchFilter)
                searchFilterCollection.Add(New SearchFilter.IsGreaterThan(ItemSchema.DateTimeReceived, DateTime.Parse(mylastdate.ToString)))
                Dim searchfilteritem As SearchFilter = New SearchFilter.SearchFilterCollection(LogicalOperator.And, searchFilterCollection.ToArray)


                For Each Folder As Folder In results.Folders
                    'Debug.Print(Folder.DisplayName)
                    If Folder.DisplayName = "Inbox" Then
                        Dim ItemView As ItemView = New ItemView(Integer.MaxValue)
                        Dim searchresult As FindItemsResults(Of Microsoft.Exchange.WebServices.Data.Item) = service.FindItems(Folder.Id, searchfilteritem, ItemView)

                        service.LoadPropertiesForItems(searchresult.Items, PropertySet.FirstClassProperties)

                       
                        For Each Item As Microsoft.Exchange.WebServices.Data.Item In searchresult.Items
                            Dim Model As New EmailModel
                            Dim sb As New StringBuilder

                            Dim myemail = DirectCast(Item, Microsoft.Exchange.WebServices.Data.EmailMessage)
                            If myemail.ToRecipients.Count > 0 Then
                                'Debug.Print(myemail.ToRecipients.Count)
                                For Each myaddress In myemail.ToRecipients
                                    If sb.Length > 0 Then
                                        sb.Append(";")
                                    End If
                                    sb.Append(myaddress.Address)
                                    'Debug.Print(myaddress.Address)
                                Next
                                Model.emailto = sb.ToString
                            End If

                            'Debug.Print(String.Format("{0:yyyyMMddHHmmss} - {1} {2} {3}", Item.DateTimeReceived, Item.Subject, myemail.From.Address, myemail.Sender.Address))
                            Model.emailsubject = myemail.Subject
                            Model.emailbody = myemail.Body.Text
                            Model.sender = myemail.From.Address
                            Model.receiveddate = String.Format("{0:yyyy-MM-dd HH:mm:ss}", myemail.DateTimeReceived)
                            'Get The latest receiveddate
                            If ds.Tables(0).Rows(0).Item("ts") < Model.receiveddate Then
                                ds.Tables(0).Rows(0).Item("ts") = Model.receiveddate
                            End If
                            If Item.HasAttachments Then
                                'Find Header If not avail then create else update
                                'Find Detail if not avail then create else update
                                'Save Attachment to specific Folder
                                Dim pkey1(2) As Object
                                pkey1(0) = Model.emailsubject
                                pkey1(1) = CDate(Model.receiveddate)
                                pkey1(2) = Model.sender

                                Dim result As DataRow = ds.Tables(1).Rows.Find(pkey1)
                                Dim myid As Long
                                If IsNothing(result) Then
                                    'Dim dr As DataRow = ds.Tables(1).NewRow
                                    Dim drv As DataRowView = bs.AddNew
                                    Dim dr As DataRow = drv.Row
                                    dr.Item("emailsubject") = Model.emailsubject
                                    dr.Item("emailbody") = Model.emailbody
                                    dr.Item("sender") = Model.sender
                                    dr.Item("sendername") = Model.sendername
                                    dr.Item("emailto") = Model.emailto
                                    dr.Item("receiveddate") = Model.receiveddate
                                    dr.Item("attn") = Model.attn
                                    myid = dr.Item("id")
                                    dr.EndEdit()
                                    ds.Tables(1).Rows.Add(dr)

                                Else
                                    myid = result.Item("id")                                    
                                    'Move BS Position so added new record in bs2 will follow BS id.
                                    Dim pos = bs.Find("id", myid)
                                    bs.Position = pos

                                End If

                                For Each Attach As Attachment In Item.Attachments
                                    Dim fileAttachment As FileAttachment = DirectCast(Attach, FileAttachment)
                                    If TypeOf Attach Is FileAttachment Then
                                        Dim pkey2(1) As Object
                                        pkey2(0) = myid
                                        pkey2(1) = fileAttachment.Name
                                        result = ds.Tables(2).Rows.Find(pkey2)
                                        If IsNothing(result) Then
                                            Dim dv As DataRowView = bs2.AddNew
                                            dv.Row.Item("attachmentname") = fileAttachment.Name
                                            ds.Tables(2).Rows.Add(dv.Row)

                                            savingfolder = mybasefolder & "\" & String.Format("{0:yyyyMM}", CDate(Model.receiveddate))
                                            If Not IO.Directory.Exists(savingfolder) Then
                                                'Check Base folder First
                                                If Not IO.Directory.Exists(mybasefolder) Then
                                                    IO.Directory.CreateDirectory(mybasefolder)
                                                End If
                                                'Create Saving Folder
                                                IO.Directory.CreateDirectory(savingfolder)
                                            End If
                                            myfilenamelog = savingfolder & "\" & String.Format("{0:ddHHmmss}", CDate(Model.receiveddate)) + fileAttachment.Name
                                            Using thestream As FileStream = New FileStream(myfilenamelog, FileMode.OpenOrCreate, FileAccess.ReadWrite)
                                                fileAttachment.Load(thestream)
                                                thestream.Close()
                                                thestream.Dispose()
                                            End Using

                                        End If
                                    End If
                                    ''savingfolder = mybasefolder & "\" & "2020" & "\" & String.Format("{0:yyyyMMddHHmmss}", CDate(Model.receiveddate))
                                    'savingfolder = mybasefolder & "\" & String.Format("{0:yyyyMM}", CDate(Model.receiveddate))
                                    'If Not IO.Directory.Exists(savingfolder) Then
                                    '    IO.Directory.CreateDirectory(savingfolder)
                                    'End If
                                    'myfilenamelog = savingfolder & "\" & String.Format("{0:ddHHmmss}", CDate(Model.receiveddate)) + fileAttachment.Name
                                    'Using thestream As FileStream = New FileStream(myfilenamelog, FileMode.OpenOrCreate, FileAccess.ReadWrite)
                                    '    fileAttachment.Load(thestream)
                                    '    thestream.Close()
                                    '    thestream.Dispose()
                                    'End Using

                                Next
                            End If


                        Next
                    End If
                Next

                'Dim ItemView As ItemView = New ItemView(Integer.MaxValue)
                'Dim searchresult As FindItemsResults(Of Microsoft.Exchange.WebServices.Data.Item) = service.FindItems(folderId, searchfilteritem, ItemView)
                'Dim searchresult As FindItemsResults(Of Microsoft.Exchange.WebServices.Data.Item) = service.FindItems(WellKnownFolderName.Inbox, searchfilteritem, ItemView)
                'Dim searchresult As FindItemsResults(Of Microsoft.Exchange.WebServices.Data.Item) = service.FindItems(results.Folders(0).Id, searchfilteritem, ItemView)
                'Dim searchresult As FindItemsResults(Of Microsoft.Exchange.WebServices.Data.Item) = service.FindItems(results.Folders(0).Id, searchfilteritem, ItemView)


                



                'Dim folder As Folder
                'For Each folder In results.Folders
                '    If TypeOf folder Is SearchFolder Then
                '        'Debug.Print("Search folder: " & TryCast(folder, SearchFolder).DisplayName)
                '        'Debug.Print("ID : " & TryCast(folder, SearchFolder).Id.ToString)
                '    ElseIf TypeOf folder Is ContactsFolder Then
                '        'Debug.Print("Search ContactsFolder: " & TryCast(folder, ContactsFolder).DisplayName)
                '    ElseIf TypeOf folder Is TasksFolder Then
                '        'Debug.Print("Search TasksFolder: " & TryCast(folder, TasksFolder).DisplayName)
                '    ElseIf TypeOf folder Is CalendarFolder Then
                '        'Debug.Print("Search CalendarFolder: " & TryCast(folder, CalendarFolder).DisplayName)
                '    ElseIf folder.DisplayName.Contains("_OLD") Then
                '        'do nothing
                '        'Debug.Print("_OLD Folder")
                '    Else

                '        'Debug.Print("Folder: " & folder.DisplayName)
                '        If Not AutoTask Then
                '            ProgressReport(2, "Read Folder: " & folder.DisplayName)
                '            ProgressReport(1, "")
                '        End If
                '        'Find item here
                '        If folder.DisplayName.Contains("Forwarder") Or
                '            folder.DisplayName.Contains("INVOICE") Or
                '            folder.DisplayName.Contains("PACKING LIST") Then
                '            Dim myfolder As String = String.Empty

                '            If folder.DisplayName.Contains("Forwarder") Then
                '                myfolder = mybasefolder & "\forwarder"
                '                mydoctype = 0
                '                mylastdate = mylastdate
                '            ElseIf folder.DisplayName.Contains("INVOICE") Then
                '                myfolder = mybasefolder & "\invoice"
                '                mydoctype = 1
                '                mylastdate = mylastdateinvoice
                '            ElseIf folder.DisplayName.Contains("PACKING LIST") Then
                '                myfolder = mybasefolder & "\packinglist"
                '                mydoctype = 2
                '                mylastdate = mylastdatepackinglist
                '            End If

                '            Dim searchFilterCollection As List(Of SearchFilter) = New List(Of SearchFilter)
                '            'search filter for Forwarder
                '            'search Filter for Invoice
                '            'search filter for packinglist
                '            searchFilterCollection.Add(New SearchFilter.IsGreaterThan(ItemSchema.DateTimeReceived, DateTime.Parse(mylastdate.ToString)))



                '            Dim searchfilteritem As SearchFilter = New SearchFilter.SearchFilterCollection(LogicalOperator.And, searchFilterCollection.ToArray)


                '            Dim ItemView As ItemView = New ItemView(Integer.MaxValue)
                '            Dim searchresult As FindItemsResults(Of Microsoft.Exchange.WebServices.Data.Item) = service.FindItems(folder.Id, searchfilteritem, ItemView)

                '            For Each Item As Microsoft.Exchange.WebServices.Data.Item In searchresult.Items

                '                If TypeOf Item Is EmailMessage Then
                '                    'Debug.Print("Email Message: " & TryCast(Item, EmailMessage).Subject)
                '                    Dim myarray = Item.Subject.Split("/")
                '                    'Update parameter emaillastreceived for forwarder,INVOICE,PACKING LIST
                '                    If folder.DisplayName.Contains("Forwarder") Then
                '                        If ds.Tables(0).Rows(0).Item("ts") < Item.DateTimeReceived Then
                '                            ds.Tables(0).Rows(0).Item("ts") = Item.DateTimeReceived
                '                        End If
                '                    ElseIf folder.DisplayName.Contains("INVOICE") Then
                '                        If ds.Tables(0).Rows(5).Item("ts") < Item.DateTimeReceived Then
                '                            ds.Tables(0).Rows(5).Item("ts") = Item.DateTimeReceived
                '                        End If
                '                    ElseIf folder.DisplayName.Contains("PACKING LIST") Then
                '                        If ds.Tables(0).Rows(6).Item("ts") < Item.DateTimeReceived Then
                '                            ds.Tables(0).Rows(6).Item("ts") = Item.DateTimeReceived
                '                        End If
                '                    End If

                '                    Dim myitems As List(Of Microsoft.Exchange.WebServices.Data.Item) = New List(Of Microsoft.Exchange.WebServices.Data.Item)
                '                    myitems.Add(Item)
                '                    service.LoadPropertiesForItems(myitems, PropertySet.FirstClassProperties)
                '                    Dim message As EmailMessage = EmailMessage.Bind(service, Item.Id, New PropertySet(BasePropertySet.FirstClassProperties, ItemSchema.Attachments))
                '                    'Debug.Print(message.From.Name & " " & message.From.Address)



                '                    If Item.HasAttachments Then
                '                        'save to db
                '                        'check header
                '                        Dim pkey1(0) As Object
                '                        'Replace any character contains ' (singlequote)
                '                        Dim mydocemailname = validfilename(myarray(myarray.Count - 1).Trim).Replace("'", "''")
                '                        If mydocemailname.Length = 0 Then
                '                            mydocemailname = "-BLANK-"
                '                        End If
                '                        pkey1(0) = mydocemailname
                '                        Dim result As DataRow = ds.Tables(3).Rows.Find(pkey1)
                '                        Dim myid As Long
                '                        If IsNothing(result) Then
                '                            'create new record
                '                            Dim dr As DataRow = ds.Tables(1).NewRow
                '                            dr.Item("docemailname") = mydocemailname
                '                            dr.Item("docemailtype") = mydoctype
                '                            dr.Item("sender") = message.From.Address
                '                            dr.Item("sendername") = message.From.Name
                '                            dr.Item("receiveddate") = Item.DateTimeReceived
                '                            dr.Item("foldername") = folder.DisplayName
                '                            myid = dr.Item("docemailhdid")
                '                            ds.Tables(1).Rows.Add(dr)
                '                            Dim mydr As DataRow = ds.Tables(3).NewRow
                '                            mydr.Item(0) = mydocemailname
                '                            mydr.Item(1) = myid
                '                            ds.Tables(3).Rows.Add(mydr)


                '                        Else
                '                            myid = result.Item(1)
                '                            Dim pkey11(0) As Object
                '                            pkey11(0) = myid
                '                            Dim myresult As DataRow = ds.Tables(1).Rows.Find(pkey11)

                '                            myresult.Item("receiveddate") = Item.DateTimeReceived
                '                            myresult.Item("sender") = message.From.Address
                '                            myresult.Item("sendername") = message.From.Name
                '                            myresult.Item("foldername") = folder.DisplayName
                '                        End If

                '                        'Dim savingfolder As String = myfolder
                '                        savingfolder = myfolder
                '                        If folder.DisplayName.Contains("Forwarder") Then
                '                            savingfolder = savingfolder & "\" & mydocemailname.ToString.Trim 'DbAdapter1.validfilename(myarray(myarray.Count - 1).Trim)
                '                            If Not Directory.Exists(savingfolder) Then
                '                                Directory.CreateDirectory(savingfolder)
                '                            End If
                '                        End If
                '                        For Each Attachment As Attachment In Item.Attachments


                '                            If TypeOf Attachment Is FileAttachment Then
                '                                Dim fileattachment As FileAttachment = DirectCast(Attachment, FileAttachment)
                '                                'fileattachment.Load() 'this one saving using original filename

                '                                'save to db
                '                                'check detail
                '                                Dim pkey2(1) As Object
                '                                pkey2(0) = fileattachment.Name
                '                                pkey2(1) = myid
                '                                result = ds.Tables(4).Rows.Find(pkey2)

                '                                If IsNothing(result) Then
                '                                    'create new record
                '                                    Dim dr As DataRow = ds.Tables(2).NewRow
                '                                    dr.Item("docemailhdid") = myid
                '                                    dr.Item("docemaildtname") = fileattachment.Name
                '                                    ds.Tables(2).Rows.Add(dr)

                '                                    Dim mydr As DataRow = ds.Tables(4).NewRow
                '                                    mydr.Item(0) = fileattachment.Name
                '                                    mydr.Item(1) = myid
                '                                    ds.Tables(4).Rows.Add(mydr)
                '                                End If

                '                                If Not AutoTask Then
                '                                    ProgressReport(1, "Attachment name: " & fileattachment.Name)
                '                                End If
                '                                'Debug.WriteLine("Attachment name: " & fileattachment.Name)
                '                                'fileattachment.Load("c:\\temp\\" + fileattachment.Name)
                '                                'Using thestream As FileStream = New FileStream("c:\\temp\\stream_" + fileattachment.Name, FileMode.OpenOrCreate, FileAccess.ReadWrite)
                '                                myfilenamelog = savingfolder + "\" + fileattachment.Name
                '                                Using thestream As FileStream = New FileStream(savingfolder + "\" + fileattachment.Name, FileMode.OpenOrCreate, FileAccess.ReadWrite)
                '                                    fileattachment.Load(thestream)
                '                                    thestream.Close()
                '                                    thestream.Dispose()
                '                                End Using
                '                            End If
                '                        Next
                '                    End If

                '                ElseIf TypeOf Item Is MeetingRequest Then
                '                    'Debug.Print("Metting Request: " & TryCast(Item, MeetingRequest).Subject)
                '                Else

                '                End If
                '            Next
                '        ElseIf folder.DisplayName.ToString = "Dell" Then
                '            Debug.Print("inside Dell")
                '        ElseIf folder.DisplayName.ToString = "DellInvoice" Then
                '            Debug.Print("inside DellINvoice")
                '        End If



                '    End If
                'Next
                If results.MoreAvailable Then
                    GetFolder01(offset + totalview)
                End If
            Catch ex As Exception
                Logg(ex.Message & " " & savingfolder & " :: " & myfilenamelog)
                ProgressReport(1, ex.Message)
                Return myreturn  'do not save the latest update
            End Try
            Dim ds2 As DataSet = ds.GetChanges

            If Not IsNothing(ds2) Then
                Dim ra As Integer
                mymessage = String.Empty

                Dim mye As New ContentBaseEventArgs(ds2, True, mymessage, ra, True)
                myModel = New EmailModel
                If Not myModel.save(Me, mye) Then
                    ProgressReport(2, "Error" & "::" & mye.message)
                    Logg(mye.message)
                    Return False
                End If
                'If Not DbAdapter1.DocEmailTx(Me, mye) Then
                '    ProgressReport(2, "Error" & "::" & mye.message)
                '    Logg(mye.message)
                '    Return False
                'End If

                'Try
                '    'Update parameters emaillastreceieved using original dataset

                '    Dim mydate As DateTime = ds.Tables(0).Rows(0).Item("ts")
                '    sqlstr = getsqlstr(mydate, "emaillastreceived", " and paramhdid = 59")
                '    If Not DbAdapter1.ExecuteNonQuery(sqlstr, ra, mymessage) Then
                '        ProgressReport(1, mymessage)
                '        Return myreturn
                '    End If

                '    Dim mydate1 As DateTime = ds.Tables(0).Rows(5).Item("ts")
                '    sqlstr = getsqlstr(mydate1, "emaillastreceivedinvoice", " and paramhdid = 59")
                '    If Not DbAdapter1.ExecuteNonQuery(sqlstr, ra, mymessage) Then
                '        ProgressReport(1, mymessage)
                '        Return myreturn
                '    End If

                '    Dim mydate2 As DateTime = ds.Tables(0).Rows(6).Item("ts")
                '    sqlstr = getsqlstr(mydate2, "emaillastreceivedpackinglist", " and paramhdid = 59")
                '    If Not DbAdapter1.ExecuteNonQuery(sqlstr, ra, mymessage) Then
                '        ProgressReport(1, mymessage)
                '        Return myreturn
                '    End If

                'Catch ex As Exception
                '    Logg(ex.Message)
                'End Try
            End If
            Return True

        End Using
    End Function


    Private Function GetFolder(ByVal offset As Integer) As Boolean
        'ProgressReport(1, "Get Folder")
        Dim mydoctype As Integer

        Dim savingfolder As String = String.Empty
        Dim myfilenamelog As String = String.Empty
        Dim myreturn As Boolean = False

        'Dim sqlstr = "select ts from paramdt where paramhdname = 'emaillastreceived';"
        'Dim sqlstr = "select dt.* from paramdt dt" &
        '             " left join paramhd hd on hd.paramhdid = dt.paramhdid" &
        '             " where hd.paramname= 'logbook'" &
        '             " order by dt.ivalue;" &
        '            " select * from docemailhd;" &
        '            " select * from docemaildt;" &
        '            " select docemailname,docemailhdid from docemailhd;" &
        '            " select docemaildtname,docemailhdid,docemaildtid from docemaildt;"
        Dim sqlstr = "select dt.* from paramdt dt" &
             " left join paramhd hd on hd.paramhdid = dt.paramhdid" &
             " where hd.paramname= 'logbook'" &
             " order by dt.ivalue;" &
            " select * from docemailhd;" &
            " select * from docemaildt;" &
            " select docemailname,docemailhdid from docemailhd;" &
            " select distinct docemaildtname,docemailhdid from docemaildt;"

        Dim mymessage As String = String.Empty
        'If Not DbAdapter1.TbgetDataSet(sqlstr, ds, mymessage) Then
        Dim ds As New DataSet
        ds = DataAccess.GetDataSet(sqlstr, CommandType.Text)
        Dim mylastdate As DateTime = ds.Tables(0).Rows(0).Item("ts")
        url = ds.Tables(0).Rows(1).Item("cvalue")
        username = ds.Tables(0).Rows(2).Item("cvalue")
        password = ds.Tables(0).Rows(3).Item("cvalue")
        mybasefolder = ds.Tables(0).Rows(4).Item("cvalue")
        Dim mylastdateinvoice As DateTime = ds.Tables(0).Rows(5).Item("ts")
        Dim mylastdatepackinglist As DateTime = ds.Tables(0).Rows(6).Item("ts")

        Try
            'ProgressReport(1, "After Get DataSet")
            'Header and Detail
            ds.Tables(1).TableName = "DocHeader"
            ds.Tables(1).CaseSensitive = True
            ds.Tables(2).TableName = "DocDtl"
            ds.Tables(2).CaseSensitive = True

            Dim idx1(0) As DataColumn
            idx1(0) = ds.Tables(1).Columns(0)
            ds.Tables(1).PrimaryKey = idx1
            ds.Tables(1).Columns(0).AutoIncrement = True
            ds.Tables(1).Columns(0).AutoIncrementSeed = -1
            ds.Tables(1).Columns(0).AutoIncrementStep = -1
            ds.Tables(1).PrimaryKey = idx1

            Dim idx2(0) As DataColumn
            idx2(0) = ds.Tables(2).Columns(0)
            ds.Tables(2).PrimaryKey = idx2
            ds.Tables(2).Columns(0).AutoIncrement = True
            ds.Tables(2).Columns(0).AutoIncrementSeed = -1
            ds.Tables(2).Columns(0).AutoIncrementStep = -1
            ds.Tables(2).PrimaryKey = idx2


            Dim rel As DataRelation
            Dim hcol As DataColumn
            Dim dcol As DataColumn

            hcol = ds.Tables(1).Columns(0) 'docemailhdid in table header
            dcol = ds.Tables(2).Columns(1) 'docemailhdid in table dtl
            rel = New DataRelation("hdrel", hcol, dcol)
            ds.Relations.Add(rel)

            bs = New BindingSource
            bs2 = New BindingSource
            bs.DataSource = ds.Tables(1)
            bs2.DataSource = bs
            bs2.DataMember = "hdrel"

            'Find Using Index For Header And Detail
            ds.Tables(3).TableName = "FindHD"
            Dim idx3(0) As DataColumn
            idx3(0) = ds.Tables(3).Columns(0) 'docemailname
            ds.Tables(3).PrimaryKey = idx3

            ds.Tables(4).TableName = "FindDT"
            Dim idx4(1) As DataColumn
            idx4(0) = ds.Tables(4).Columns(0) 'docemaildtname
            idx4(1) = ds.Tables(4).Columns(1) 'doceamilhdid
            ds.Tables(4).CaseSensitive = True
            ds.Tables(4).PrimaryKey = idx4


        Catch ex As Exception
            Logg(ex.Message)
            ProgressReport(1, ex.Message)
            Return myreturn
        End Try

        Dim totalview As Integer = Integer.MaxValue
        'totalview = IIf(IsNumeric(TextBox7.Text), CInt(TextBox7.Text), 0)
        'ProgressReport(1, "Using Service")
        Using myservice As New ClassEWS(url, username, password, "as", False)
            service = myservice.CreateConnection()

            Dim view As FolderView = New FolderView(totalview)
            view.PropertySet = New PropertySet(BasePropertySet.IdOnly)
            view.PropertySet.Add(FolderSchema.DisplayName)
            view.Offset = offset
            'MessageBox.Show(view.Offset)
            Dim searchFilter As SearchFilter = New SearchFilter.IsGreaterThan(FolderSchema.TotalCount, 0)
            view.Traversal = FolderTraversal.Deep
            Try
                'Dim results As FindFoldersResults = service.FindFolders(WellKnownFolderName.Root, searchFilter, view)
                'Dim results As FindFoldersResults = service.FindFolders(WellKnownFolderName.Inbox, New FolderView(Integer.MaxValue) With {.Traversal = FolderTraversal.Deep})
                'Dim results As FindFoldersResults = service.FindFolders(WellKnownFolderName.Inbox, searchFilter, view)
                Dim userMailbox = New Mailbox("HONsscap@groupeseb.com") 'New Mailbox("dlie@groupeseb.com") 'New Mailbox("sebshipdoc@groupeseb.com") 'New Mailbox("HONsscap@groupeseb.com")
                Dim folderId = New FolderId(WellKnownFolderName.Inbox, userMailbox) 'FolderId(WellKnownFolderName.Root, userMailbox) 'New FolderId(WellKnownFolderName.Inbox, userMailbox)

                Dim results As FindFoldersResults = service.FindFolders(folderId, searchFilter, view)
                Dim folder As Folder
                For Each folder In results.Folders
                    If TypeOf folder Is SearchFolder Then
                        'Debug.Print("Search folder: " & TryCast(folder, SearchFolder).DisplayName)
                        'Debug.Print("ID : " & TryCast(folder, SearchFolder).Id.ToString)
                    ElseIf TypeOf folder Is ContactsFolder Then
                        'Debug.Print("Search ContactsFolder: " & TryCast(folder, ContactsFolder).DisplayName)
                    ElseIf TypeOf folder Is TasksFolder Then
                        'Debug.Print("Search TasksFolder: " & TryCast(folder, TasksFolder).DisplayName)
                    ElseIf TypeOf folder Is CalendarFolder Then
                        'Debug.Print("Search CalendarFolder: " & TryCast(folder, CalendarFolder).DisplayName)
                    ElseIf folder.DisplayName.Contains("_OLD") Then
                        'do nothing
                        'Debug.Print("_OLD Folder")
                    Else

                        'Debug.Print("Folder: " & folder.DisplayName)
                        If Not AutoTask Then
                            ProgressReport(2, "Read Folder: " & folder.DisplayName)
                            ProgressReport(1, "")
                        End If
                        'Find item here
                        If folder.DisplayName.Contains("Forwarder") Or
                            folder.DisplayName.Contains("INVOICE") Or
                            folder.DisplayName.Contains("PACKING LIST") Then
                            Dim myfolder As String = String.Empty

                            If folder.DisplayName.Contains("Forwarder") Then
                                myfolder = mybasefolder & "\forwarder"
                                mydoctype = 0
                                mylastdate = mylastdate
                            ElseIf folder.DisplayName.Contains("INVOICE") Then
                                myfolder = mybasefolder & "\invoice"
                                mydoctype = 1
                                mylastdate = mylastdateinvoice
                            ElseIf folder.DisplayName.Contains("PACKING LIST") Then
                                myfolder = mybasefolder & "\packinglist"
                                mydoctype = 2
                                mylastdate = mylastdatepackinglist
                            End If

                            Dim searchFilterCollection As List(Of SearchFilter) = New List(Of SearchFilter)
                            'search filter for Forwarder
                            'search Filter for Invoice
                            'search filter for packinglist
                            searchFilterCollection.Add(New SearchFilter.IsGreaterThan(ItemSchema.DateTimeReceived, DateTime.Parse(mylastdate.ToString)))



                            Dim searchfilteritem As SearchFilter = New SearchFilter.SearchFilterCollection(LogicalOperator.And, searchFilterCollection.ToArray)


                            Dim ItemView As ItemView = New ItemView(Integer.MaxValue)
                            Dim searchresult As FindItemsResults(Of Microsoft.Exchange.WebServices.Data.Item) = service.FindItems(folder.Id, searchfilteritem, ItemView)

                            For Each Item As Microsoft.Exchange.WebServices.Data.Item In searchresult.Items

                                If TypeOf Item Is EmailMessage Then
                                    'Debug.Print("Email Message: " & TryCast(Item, EmailMessage).Subject)
                                    Dim myarray = Item.Subject.Split("/")
                                    'Update parameter emaillastreceived for forwarder,INVOICE,PACKING LIST
                                    If folder.DisplayName.Contains("Forwarder") Then
                                        If ds.Tables(0).Rows(0).Item("ts") < Item.DateTimeReceived Then
                                            ds.Tables(0).Rows(0).Item("ts") = Item.DateTimeReceived
                                        End If
                                    ElseIf folder.DisplayName.Contains("INVOICE") Then
                                        If ds.Tables(0).Rows(5).Item("ts") < Item.DateTimeReceived Then
                                            ds.Tables(0).Rows(5).Item("ts") = Item.DateTimeReceived
                                        End If
                                    ElseIf folder.DisplayName.Contains("PACKING LIST") Then
                                        If ds.Tables(0).Rows(6).Item("ts") < Item.DateTimeReceived Then
                                            ds.Tables(0).Rows(6).Item("ts") = Item.DateTimeReceived
                                        End If
                                    End If

                                    Dim myitems As List(Of Microsoft.Exchange.WebServices.Data.Item) = New List(Of Microsoft.Exchange.WebServices.Data.Item)
                                    myitems.Add(Item)
                                    service.LoadPropertiesForItems(myitems, PropertySet.FirstClassProperties)
                                    Dim message As EmailMessage = EmailMessage.Bind(service, Item.Id, New PropertySet(BasePropertySet.FirstClassProperties, ItemSchema.Attachments))
                                    'Debug.Print(message.From.Name & " " & message.From.Address)



                                    If Item.HasAttachments Then
                                        'save to db
                                        'check header
                                        Dim pkey1(0) As Object
                                        'Replace any character contains ' (singlequote)
                                        Dim mydocemailname = validfilename(myarray(myarray.Count - 1).Trim).Replace("'", "''")
                                        If mydocemailname.Length = 0 Then
                                            mydocemailname = "-BLANK-"
                                        End If
                                        pkey1(0) = mydocemailname
                                        Dim result As DataRow = ds.Tables(3).Rows.Find(pkey1)
                                        Dim myid As Long
                                        If IsNothing(result) Then
                                            'create new record
                                            Dim dr As DataRow = ds.Tables(1).NewRow
                                            dr.Item("docemailname") = mydocemailname
                                            dr.Item("docemailtype") = mydoctype
                                            dr.Item("sender") = message.From.Address
                                            dr.Item("sendername") = message.From.Name
                                            dr.Item("receiveddate") = Item.DateTimeReceived
                                            dr.Item("foldername") = folder.DisplayName
                                            myid = dr.Item("docemailhdid")
                                            ds.Tables(1).Rows.Add(dr)
                                            Dim mydr As DataRow = ds.Tables(3).NewRow
                                            mydr.Item(0) = mydocemailname
                                            mydr.Item(1) = myid
                                            ds.Tables(3).Rows.Add(mydr)


                                        Else
                                            myid = result.Item(1)
                                            Dim pkey11(0) As Object
                                            pkey11(0) = myid
                                            Dim myresult As DataRow = ds.Tables(1).Rows.Find(pkey11)

                                            myresult.Item("receiveddate") = Item.DateTimeReceived
                                            myresult.Item("sender") = message.From.Address
                                            myresult.Item("sendername") = message.From.Name
                                            myresult.Item("foldername") = folder.DisplayName                                            
                                        End If

                                        'Dim savingfolder As String = myfolder
                                        savingfolder = myfolder
                                        If folder.DisplayName.Contains("Forwarder") Then
                                            savingfolder = savingfolder & "\" & mydocemailname.ToString.Trim 'DbAdapter1.validfilename(myarray(myarray.Count - 1).Trim)
                                            If Not Directory.Exists(savingfolder) Then
                                                Directory.CreateDirectory(savingfolder)
                                            End If
                                        End If
                                        For Each Attachment As Attachment In Item.Attachments


                                            If TypeOf Attachment Is FileAttachment Then
                                                Dim fileattachment As FileAttachment = DirectCast(Attachment, FileAttachment)
                                                'fileattachment.Load() 'this one saving using original filename

                                                'save to db
                                                'check detail
                                                Dim pkey2(1) As Object
                                                pkey2(0) = fileattachment.Name
                                                pkey2(1) = myid
                                                result = ds.Tables(4).Rows.Find(pkey2)

                                                If IsNothing(result) Then
                                                    'create new record
                                                    Dim dr As DataRow = ds.Tables(2).NewRow
                                                    dr.Item("docemailhdid") = myid
                                                    dr.Item("docemaildtname") = fileattachment.Name
                                                    ds.Tables(2).Rows.Add(dr)

                                                    Dim mydr As DataRow = ds.Tables(4).NewRow
                                                    mydr.Item(0) = fileattachment.Name
                                                    mydr.Item(1) = myid
                                                    ds.Tables(4).Rows.Add(mydr)
                                                End If

                                                If Not AutoTask Then
                                                    ProgressReport(1, "Attachment name: " & fileattachment.Name)
                                                End If
                                                'Debug.WriteLine("Attachment name: " & fileattachment.Name)
                                                'fileattachment.Load("c:\\temp\\" + fileattachment.Name)
                                                'Using thestream As FileStream = New FileStream("c:\\temp\\stream_" + fileattachment.Name, FileMode.OpenOrCreate, FileAccess.ReadWrite)
                                                myfilenamelog = savingfolder + "\" + fileattachment.Name
                                                Using thestream As FileStream = New FileStream(savingfolder + "\" + fileattachment.Name, FileMode.OpenOrCreate, FileAccess.ReadWrite)
                                                    fileattachment.Load(thestream)
                                                    thestream.Close()
                                                    thestream.Dispose()
                                                End Using
                                            End If
                                        Next
                                    End If

                                ElseIf TypeOf Item Is MeetingRequest Then
                                    'Debug.Print("Metting Request: " & TryCast(Item, MeetingRequest).Subject)
                                Else

                                End If
                            Next
                        ElseIf folder.DisplayName.ToString = "Dell" Then
                            Debug.Print("inside Dell")
                        ElseIf folder.DisplayName.ToString = "DellInvoice" Then
                            Debug.Print("inside DellINvoice")
                        End If



                    End If
                Next
                If results.MoreAvailable Then
                    GetFolder(offset + totalview)
                End If
            Catch ex As Exception
                Logg(ex.Message & " " & savingfolder & " :: " & myfilenamelog)
                ProgressReport(1, ex.Message)
                Return myreturn  'do not save the latest update
            End Try
            Dim ds2 As DataSet = ds.GetChanges

            If Not IsNothing(ds2) Then
                'Dim ra As Integer
                'mymessage = String.Empty

                'Dim mye As New ContentBaseEventArgs(ds2, True, mymessage, ra, True)
                'If Not DbAdapter1.DocEmailTx(Me, mye) Then
                '    ProgressReport(2, "Error" & "::" & mye.message)
                '    Logg(mye.message)
                '    Return False
                'End If

                'Try
                '    'Update parameters emaillastreceieved using original dataset

                '    Dim mydate As DateTime = ds.Tables(0).Rows(0).Item("ts")
                '    sqlstr = getsqlstr(mydate, "emaillastreceived", " and paramhdid = 59")
                '    If Not DbAdapter1.ExecuteNonQuery(sqlstr, ra, mymessage) Then
                '        ProgressReport(1, mymessage)
                '        Return myreturn
                '    End If

                '    Dim mydate1 As DateTime = ds.Tables(0).Rows(5).Item("ts")
                '    sqlstr = getsqlstr(mydate1, "emaillastreceivedinvoice", " and paramhdid = 59")
                '    If Not DbAdapter1.ExecuteNonQuery(sqlstr, ra, mymessage) Then
                '        ProgressReport(1, mymessage)
                '        Return myreturn
                '    End If

                '    Dim mydate2 As DateTime = ds.Tables(0).Rows(6).Item("ts")
                '    sqlstr = getsqlstr(mydate2, "emaillastreceivedpackinglist", " and paramhdid = 59")
                '    If Not DbAdapter1.ExecuteNonQuery(sqlstr, ra, mymessage) Then
                '        ProgressReport(1, mymessage)
                '        Return myreturn
                '    End If

                'Catch ex As Exception
                '    Logg(ex.Message)
                'End Try
            End If
            Return True

        End Using
    End Function

    Public Sub New()

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        If AutoTask Then
            'HelperClass1 = New HelperClass
            'DbAdapter1 = New DbAdapter
        End If
    End Sub
    Public Sub New(ByVal AutoTask As Boolean)

        ' This call is required by the designer.
        InitializeComponent()
        Me.AutoTask = AutoTask
        ' Add any initialization after the InitializeComponent() call.

    End Sub



    Private Sub Logg(ByVal mymessage As String)
        If AutoTask Then
            Logger.log(mymessage)
        End If
    End Sub

    Private Function getsqlstr(ByVal mydate As Date, ByVal paramname As String, Optional ByVal mycriteria As String = "") As String
        Dim myvaliddate = "'" & mydate.Year & "-" & mydate.Month & "-" & mydate.Day & " " & mydate.Hour & ":" & mydate.Minute & ":" & mydate.Second & "'"
        Dim sqlstr = "update paramdt set ts = " & myvaliddate & " where paramdt.paramname = '" & paramname & "'" & mycriteria & ";"
        Return sqlstr
    End Function

    Private Sub FormGetEmailFromExServer_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        If myThread.IsAlive Then
            MessageBox.Show("Please wait until the current process is finished.")
            e.Cancel = True
        End If
    End Sub

    Private Sub FormGetEmailFromExServer_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

    End Sub

    Private Sub Button1_Click_1(sender As Object, e As EventArgs) Handles Button1.Click

    End Sub

    Function checkLockFile(ByVal path As String) As Boolean
        Dim myret As Boolean = False
        If File.Exists(path) Then
            myret = True
        Else
            'create file
            Using fs As FileStream = File.Create(path)
                Dim info As Byte() = New UTF8Encoding(True).GetBytes("0")
                ' Add some information to the file.
                fs.Write(info, 0, info.Length)
                fs.Close()
            End Using
        End If
        Return myret
    End Function

    Public Function validfilename(ByVal strToReplace As String) As String
        Dim mychar() As String = {"\", "/", ":", "*", "?", "<", ">", "|", """"}
        For Each value As String In mychar
            strToReplace = strToReplace.Replace(value, " ")
        Next
        Return strToReplace
    End Function

   
End Class