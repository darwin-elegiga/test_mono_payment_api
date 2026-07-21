package templates

#ServiceAccount: {
	apiVersion: "v1"
	kind:       "ServiceAccount"
	metadata: {
		name:      string
		namespace: string
		labels?: {
			[string]: string
		}
	}
}
