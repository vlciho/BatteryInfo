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
        FILE_NAME = appPath & "\BatteryInfo-log.txt"

        Dim writer As StreamWriter = New StreamWriter(File.Open(FILE_NAME, FileMode.Append))

        writer.WriteLine(Date.Now.ToString + ": " + "")
        writer.WriteLine(Date.Now.ToString + ": " + "", True)
        writer.WriteLine(Date.Now.ToString + ": " + " S T A R T ", True)
        writer.WriteLine(Date.Now.ToString + ": " + "", True)
        writer.WriteLine(Date.Now.ToString + ": " + "DolniMez: " + My.Settings.dolniMez.ToString + "%; HorniMez: " + My.Settings.horniMez.ToString + "%", True)

        Try
            Dim result As String = webClient.DownloadString(proved)
        Catch ex As Exception
            If strDebug Then
                Console.WriteLine(ex.Message)
                writer.WriteLine(Date.Now.ToString + ": " + ex.Message.ToString, True)
            End If
        End Try

        proved = ""

        ps = SystemInformation.PowerStatus
        Dim plf As Single = ps.BatteryLifePercent
        Dim nabito As Integer = (plf * 100)
        Dim horniMez As Integer = My.Settings.horniMez
        Dim dolniMez As Integer = My.Settings.dolniMez

        webClient.Encoding = System.Text.Encoding.UTF8

        'Console.WriteLine("Battery charge status: Charging")
        'Console.WriteLine("Battery level: " & output)

        proved = "NIC"

        'Vypnout zásuvku
        If nabito >= horniMez Then proved = My.Settings.wifiZasuvkaOFF.ToString

        'Zapnout zásuvku
        If nabito <= dolniMez Then proved = My.Settings.wifiZasuvkaON.ToString

        writer.WriteLine(Date.Now.ToString + ": " + "Nabito: " + nabito.ToString, True)
        writer.WriteLine(Date.Now.ToString + ": " + proved, True)

        Try
            If strDebug Then
                writer.WriteLine(Date.Now.ToString + ": " + nabito.ToString + ";" + proved.ToString, True)
            End If

            Dim result As String = webClient.DownloadString(proved)
            proved = ""

            'Console.WriteLine(result)
            If strDebug Then
                writer.WriteLine(Date.Now.ToString + ": " + result, True)
            End If

        Catch ex As Exception
            'Console.WriteLine(ex.Message)
        End Try

        writer.Flush()
        writer.Close()
        writer = Nothing


    End Sub

End Module
