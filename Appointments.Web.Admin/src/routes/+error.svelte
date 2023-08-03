<script>
	import { page } from '$app/stores';
	import { ErrorPage, message, statusCode } from '$lib/error-page';

	message.set($page.error?.message ?? 'An unexpected error occurred');
	statusCode.set($page.status);
</script>

<div class="container py-5">
	<ErrorPage />
</div>
