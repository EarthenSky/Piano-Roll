Imports WMPLib 'Media Player in virtual form
Imports System.IO

Enum Note
    Gsh5 = 831
    G5 = 784
    Fsh5 = 740
    F5 = 698
    E5 = 659
    Dsh5 = 622
    D5 = 587
    Csh5 = 554
    C5 = 523
    B4 = 494
    Ash4 = 466
    A4 = 440
End Enum

Public Class Form1
    'Piano Roll 
    'Gabe Stang
    '
    'Play a piano on the piano roll

    Const shtBlockSizeX As Short = 8
    Const shtBlockSizeY As Short = 32
    Const shtSpaceToLeft As Short = 0
    Const shtSpaceToTop As Short = 0

    Private arrayValue(512, 60) As Integer '1 is start of note, 2 is body, 3 is end, 0 is no note.

    Private Sub Form1_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

    End Sub

    Private Sub MouseDownClick(ByVal sender As System.Object, ByVal e As MouseEventArgs) Handles pnlGrid.MouseDown
        If e.Button = Windows.Forms.MouseButtons.Left Then
            Dim xIndex = (e.X + shtSpaceToLeft) \ shtBlockSizeX
            Dim yIndex = (e.Y + shtSpaceToTop) \ shtBlockSizeY
            Me.Text = "X: " & xIndex & " Y: " & yIndex

            If yIndex = 0 Then
                PlaySound(7000, 50)
            ElseIf yIndex = 1 Then
                PlaySound(Note.E5, 50)
            ElseIf yIndex = 2 Then
                PlaySound(Note.Dsh5, 50)
            ElseIf yIndex = 3 Then
                PlaySound(Note.D5, 50)
            ElseIf yIndex = 4 Then
                PlaySound(Note.Csh5, 50)
            ElseIf yIndex = 5 Then
                PlaySound(Note.C5, 50)
            ElseIf yIndex = 6 Then
                PlaySound(Note.B4, 50)
            ElseIf yIndex = 7 Then
                PlaySound(Note.Ash4, 50)
            ElseIf yIndex = 8 Then
                PlaySound(Note.A4, 50)
            End If

            'places a note at the specific place clicked
            'FindAndDeleteNote(xIndex, yIndex)
            arrayValue(xIndex, yIndex) = 1

            'FindAndDeleteNote(xIndex + 1, yIndex)
            arrayValue(xIndex + 1, yIndex) = 3
        End If
    End Sub

    Private Sub FindAndDeleteNote(ByVal xIndex As Short, ByVal yIndex As Short)
        Dim shtCurrentPlace As Short = 0
        If arrayValue(xIndex, yIndex) <> 0 Then
            If arrayValue(xIndex, yIndex) = 1 Then
                While arrayValue(xIndex + shtCurrentPlace, yIndex) <> 3  'check down the row if it is a part of a note and delete it.
                    arrayValue(xIndex + shtCurrentPlace, yIndex) = 0
                    shtCurrentPlace += 1
                End While
                arrayValue(xIndex + shtCurrentPlace, yIndex) = 0

            ElseIf arrayValue(xIndex, yIndex) = 2 Then
                While arrayValue(xIndex + shtCurrentPlace, yIndex) <> 0
                    If arrayValue(xIndex + shtCurrentPlace, yIndex) = 2 Then
                        arrayValue(xIndex + shtCurrentPlace, yIndex) = 0
                        shtCurrentPlace -= 1
                    ElseIf arrayValue(xIndex + shtCurrentPlace, yIndex) = 1 Then

                    End If

                End While

            End If
        End If
    End Sub

    Private Sub Button1Click() Handles Button1.Click
        PlaySound(Note.A4, 50)
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
