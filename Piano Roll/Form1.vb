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

    Const shtBlockSize As Short = 8

    Private arrayValue(512, 60) As Integer '1 is start of note, 2 is body, 3 is end, 0 is no note.

    Private Sub Form1_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

    End Sub

    Private Sub MouseDownClick(ByVal sender As System.Object, ByVal e As MouseEventArgs) Handles pnlGrid.MouseDown
        If e.Button = Windows.Forms.MouseButtons.Left Then
            Dim xIndex = e.X + 6 \ shtBlockSize
            Dim yIndex = e.Y + 6 \ shtBlockSize

            FindAndDeleteNote(xIndex, yIndex)
            arrayValue(xIndex, yIndex) = 1

            FindAndDeleteNote(xIndex + 1, yIndex)
            arrayValue(xIndex + 1, yIndex) = 3
        End If
    End Sub

    Private Sub FindAndDeleteNote(ByVal xIndex As Short, ByVal yIndex As Short)
        If arrayValue(xIndex, yIndex) <> 0 Then
            If arrayValue(xIndex, yIndex) = 1 Then
                While arrayValue(xIndex, yIndex) <> 3

                End While
            End If
        End If
    End Sub

    Private Sub Button1Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        PlaySound(Note.A4, 100)
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
