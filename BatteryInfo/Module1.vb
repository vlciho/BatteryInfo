Imports System.IO
Imports System.Management
Imports System.Net
Imports System.Windows
Imports System.Windows.Forms
Imports System.Configuration


Module Module1

    Dim proved As String
    Dim webClient As New System.Net.WebClient
    Dim ps As PowerStatus
    Dim strDebug As Boolean
    Dim appPath As String
    Dim exePath As String
    Dim FILE_NAME As String

    Sub Main()

        strDebug = My.Settings.debug
        proved = My.Settings.wifiZasuvkaON.ToString
        appPath = Application.StartupPath()
        exePath = Application.ExecutablePath()
        Dim horniMez As Integer = My.Settings.horniMez
        Dim dolniMez As Integer = My.Settings.dolniMez


        FILE_NAME = appPath & "\BatteryInfo-log.txt"

        Dim writer As StreamWriter = New StreamWriter(File.Open(FILE_NAME, FileMode.Append))

        writer.WriteLine(Date.Now.ToString + ": " + "", True)
        writer.WriteLine(Date.Now.ToString + ": " + "DolniMez: " + dolniMez.ToString + "%; HorniMez: " + horniMez.ToString + "%", True)


        ps = SystemInformation.PowerStatus
        Dim plf As Single = ps.BatteryLifePercent
        Dim nabito As Integer = (plf * 100)

        webClient.Encoding = System.Text.Encoding.UTF8

        'Console.WriteLine("Battery charge status: Charging")
        'Console.WriteLine("Battery level: " & output)

        proved = "NIC"

        'Vypnout zásuvku
        If nabito >= horniMez Then proved = My.Settings.wifiZasuvkaOFF.ToString

        'Zapnout zásuvku
        If nabito <= dolniMez Then proved = My.Settings.wifiZasuvkaON.ToString

        'writer.WriteLine(Date.Now.ToString + ": " + "Nabito: " + nabito.ToString, True)
        'writer.WriteLine(Date.Now.ToString + ": " + proved, True)

        Try
            If strDebug Then
                writer.WriteLine(Date.Now.ToString + ": Nabito: " + nabito.ToString + ";" + proved.ToString, True)
            End If

            If proved <> "NIC" Then
                Dim result As String = webClient.DownloadString(proved)
                proved = ""
                'Console.WriteLine(result)
                If strDebug Then
                    writer.WriteLine(Date.Now.ToString + ": " + result, True)
                End If
            End If

        Catch ex As Exception
            'Console.WriteLine(ex.Message)
            writer.WriteLine(ex.Message, True)
        End Try

        writer.Flush()
        writer.Close()
        writer = Nothing

        proved = Nothing
        webClient = Nothing
        ps = Nothing
        strDebug = Nothing
        appPath = Nothing
        exePath = Nothing
        FILE_NAME = Nothing

        Exit Sub
    End Sub

End Module
