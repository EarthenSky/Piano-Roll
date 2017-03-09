Imports WMPLib 'Media Player in virtual form
Imports System.IO

Enum Note
    B4 = 494
    Ash4 = 466
    A4 = 440
End Enum

Public Class Form1
    'Piano Roll
    'Gabe Stang
    '
    'Play a piano on the piano roll

    Private multiDimValue As Integer()()

    Private Sub Form1_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

    End Sub

    Private Sub MouseDownClick() Handles MyBase.MouseClick

    End Sub

    Private Sub Button1Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        Console.Beep(440, 100) 'A4 
    End Sub
    ''' <summary>
    ''' Plays a sound based on the note and the length
    ''' </summary>
    ''' <param name="length">Input a number like 7/32</param>
    Private Sub PlaySound(ByVal note As Note, ByVal length As Short)
        Console.Beep(note, length)
    End Sub

End Class

'//           ▄███▄  /
'//           ██▄██▄ -- <Quack!>
'//       ▄█▄▄▄███▄  \
'//       ██████████
'//       ▀███████▀
'//   ________________________
'//  |CODE DUCK STRIKES AGAIN!|
'//  |________________________|
