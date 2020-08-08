Imports System.IO
Imports System.Management
Imports System.Net
Imports System.Windows
Imports System.Windows.Forms

Module Module1

    Dim proved As String
    Dim webClient As New System.Net.WebClient
    Dim ps As PowerStatus

    Sub Main()

        aTimer.AutoReset = True
        aTimer.Interval = 180000 '3 minuty
        AddHandler aTimer.Elapsed, AddressOf BatteryInfo
        aTimer.Start()
        Application.Run()

    End Sub

    Dim aTimer As New System.Timers.Timer

    Public Sub BatteryInfo(ByVal sender As Object, ByVal e As System.Timers.ElapsedEventArgs)

        ps = SystemInformation.PowerStatus
        Dim plf As Single = ps.BatteryLifePercent
        Dim output As String = (plf * 100).ToString & "%"
        webClient.Encoding = System.Text.Encoding.UTF8

        'Console.WriteLine("Battery charge status: Charging")
        'Console.WriteLine("Battery level: " & output)

        'Vypnout zásuvku
        If output >= 100 Then proved = My.Settings.wifiZasuvkaOFF.ToString

        'Zapnout zásuvku
        If output <= 15 Then proved = My.Settings.wifiZasuvkaOFF.ToString

        Try
            Dim result As String = webClient.DownloadString(proved)

            Console.WriteLine(result)
            'MsgBox(result)

        Catch ex As Exception
            Console.WriteLine(ex.Message)
        End Try

    End Sub

End Module
