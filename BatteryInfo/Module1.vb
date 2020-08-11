Imports System.IO
Imports System.Management
Imports System.Net
Imports System.Windows
Imports System.Windows.Forms

Module Module1

    Dim proved As String
    Dim webClient As New System.Net.WebClient
    Dim ps As PowerStatus
    Dim strDebug As Boolean

    Sub Main()

        'Logger.LogInfo("Starting PocasiRT " + Application.ProductVersion.ToString)
        strDebug = My.Settings.debug
        proved = My.Settings.wifiZasuvkaON.ToString

        Try
            Dim result As String = webClient.DownloadString(proved)
        Catch ex As Exception
            If strDebug Then Console.WriteLine(ex.Message)
        End Try

        proved = ""

        aTimer.AutoReset = True
        aTimer.Interval = My.Settings.perioda       '2 minuty
        AddHandler aTimer.Elapsed, AddressOf BatteryInfo
        aTimer.Start()
        Application.Run()

    End Sub

    Dim aTimer As New System.Timers.Timer

    Public Sub BatteryInfo(ByVal sender As Object, ByVal e As System.Timers.ElapsedEventArgs)

        'Logger.LogInfo("Tick")

        ps = SystemInformation.PowerStatus
        Dim plf As Single = ps.BatteryLifePercent
        Dim nabito As Integer = (plf * 100)
        Dim horniMez As Integer = My.Settings.horniMez
        Dim dolniMez As Integer = My.Settings.dolniMez

        webClient.Encoding = System.Text.Encoding.UTF8

        'Console.WriteLine("Battery charge status: Charging")
        'Console.WriteLine("Battery level: " & output)

        'Vypnout zásuvku
        If nabito >= horniMez Then proved = My.Settings.wifiZasuvkaOFF.ToString

        'Zapnout zásuvku
        If nabito <= dolniMez Then proved = My.Settings.wifiZasuvkaON.ToString

        Try
            If strDebug Then MsgBox(nabito.ToString + ";" + proved.ToString)

            Dim result As String = webClient.DownloadString(proved)

            'Console.WriteLine(result)
            If strDebug Then MsgBox(result)

        Catch ex As Exception
            'Console.WriteLine(ex.Message)
        End Try

    End Sub

End Module
