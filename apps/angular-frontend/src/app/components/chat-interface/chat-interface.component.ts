// src/app/components/chat-interface/chat-interface.component.ts
import { Component, ChangeDetectionStrategy, inject, signal, WritableSignal, effect, ViewChild, ElementRef } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';

import { SupportApiService } from '../../services/support-api.service';
import { WebsocketService } from '../../services/websocket.service';
import { ChatMessage } from '../../interfaces/chat.interface';

@Component({
  selector: 'app-chat-interface',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './chat-interface.component.html',
  styleUrls: ['./chat-interface.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class ChatInterfaceComponent {
  private readonly apiService = inject(SupportApiService);
  public readonly websocketService = inject(WebsocketService);

  messages: WritableSignal<ChatMessage[]> = signal([]);
  userInput: WritableSignal<string> = signal('');
  isLoading: WritableSignal<boolean> = signal(false);

  @ViewChild('chatContainer') private chatContainer!: ElementRef;

  constructor() {
    effect(() => { if (this.messages()) this.scrollToBottom(); });

    this.websocketService.progressUpdates$
      .pipe(takeUntilDestroyed())
      .subscribe(update => {
        // FIX: Check if the update contains a gRPC error from the backend.
        if (update.error) {
          const errorMessage: ChatMessage = {
            id: Date.now(),
            sender: 'error',
            text: `A backend error occurred: ${update.error.message} (Code: ${update.error.code})`,
            timestamp: new Date(),
          };
          this.messages.update(msgs => [...msgs, errorMessage]);
        } else {
          const systemMessage: ChatMessage = {
            id: Date.now(),
            sender: 'system',
            text: `[${update.percentage_complete}%] ${update.message}`,
            timestamp: new Date(),
          };
          this.messages.update(msgs => [...msgs, systemMessage]);
        }

        if (update.status === 'COMPLETED' || update.status === 'FAILED') {
          this.isLoading.set(false);
        }
      });
  }

  sendMessage(): void {
    const query = this.userInput().trim();
    const channelName = this.websocketService.currentChannelName();
    if (!query || this.isLoading() || !channelName) return;

    const userMessage: ChatMessage = { id: Date.now(), text: query, sender: 'user', timestamp: new Date() };
    this.messages.update(msgs => [...msgs, userMessage]);
    this.userInput.set('');
    this.isLoading.set(true);

    this.apiService.sendQuery({ query, channel_name: channelName })
      .subscribe({
        next: (response) => {
          const assistantMessage: ChatMessage = { id: Date.now() + 1, text: response.initial_response, sender: 'assistant', timestamp: new Date() };
          this.messages.update(msgs => [...msgs, assistantMessage]);
        },
        error: (err) => {
          console.error("API Error:", err);
          const errorMessage: ChatMessage = { id: Date.now() + 1, text: err.error?.details || 'Sorry, I encountered an API error. Please check the Django service.', sender: 'error', timestamp: new Date() };
          this.messages.update(msgs => [...msgs, errorMessage]);
          this.isLoading.set(false);
        }
      });
  }

  cancelOperation(): void {
    this.websocketService.sendCancelRequest();
  }

  private scrollToBottom(): void {
    try {
      setTimeout(() => { this.chatContainer.nativeElement.scrollTop = this.chatContainer.nativeElement.scrollHeight; }, 0);
    } catch (err) {}
  }
}
